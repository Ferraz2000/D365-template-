using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Common;

namespace Template.Plugins.Tests.Fakes
{
    /// <summary>
    /// Harness de testes sem dependência externa: simula o pipeline (IServiceProvider,
    /// IPluginExecutionContext) e um IOrganizationService em memória. Roda em qualquer
    /// runtime (.NET Framework, Mono, CI) — não usa FakeXrmEasy.
    /// </summary>
    public sealed class PluginHarness
    {
        public FakeOrganizationService Service { get; } = new FakeOrganizationService();
        public FakeTracingService Tracing { get; } = new FakeTracingService();
        public FakeNotificationService Notifications { get; } = new FakeNotificationService();

        public FakePluginContext Context(string message, int stage, string primaryEntityName = null) =>
            new FakePluginContext { MessageName = message, Stage = stage, PrimaryEntityName = primaryEntityName };

        public void Execute<TPlugin>(FakePluginContext context) where TPlugin : IPlugin, new()
        {
            var provider = new FakeServiceProvider(context, Tracing, new FakeOrganizationServiceFactory(Service), Notifications);
            new TPlugin().Execute(provider);
        }
    }

    public sealed class FakeNotificationService : IServiceEndpointNotificationService
    {
        public int Chamadas { get; private set; }
        public string Execute(EntityReference serviceEndpoint, IExecutionContext context)
        {
            Chamadas++;
            return "ok";
        }
    }

    public sealed class FakeServiceProvider : IServiceProvider
    {
        private readonly IPluginExecutionContext _context;
        private readonly ITracingService _tracing;
        private readonly IOrganizationServiceFactory _factory;
        private readonly IServiceEndpointNotificationService _notifications;

        public FakeServiceProvider(IPluginExecutionContext context, ITracingService tracing, IOrganizationServiceFactory factory, IServiceEndpointNotificationService notifications)
        {
            _context = context;
            _tracing = tracing;
            _factory = factory;
            _notifications = notifications;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IPluginExecutionContext)) return _context;
            if (serviceType == typeof(ITracingService)) return _tracing;
            if (serviceType == typeof(IOrganizationServiceFactory)) return _factory;
            if (serviceType == typeof(IServiceEndpointNotificationService)) return _notifications;
            return null;
        }
    }

    public sealed class FakeOrganizationServiceFactory : IOrganizationServiceFactory
    {
        private readonly IOrganizationService _service;
        public FakeOrganizationServiceFactory(IOrganizationService service) => _service = service;
        public IOrganizationService CreateOrganizationService(Guid? userId) => _service;
    }

    public sealed class FakeTracingService : ITracingService
    {
        public List<string> Logs { get; } = new List<string>();
        public void Trace(string format, params object[] args) =>
            Logs.Add(args != null && args.Length > 0 ? string.Format(format, args) : format);
    }

    /// <summary>IOrganizationService em memória — cobre Create/Retrieve/Update/Delete/RetrieveMultiple.</summary>
    public sealed class FakeOrganizationService : IOrganizationService
    {
        private readonly Dictionary<string, Entity> _store = new Dictionary<string, Entity>();
        private static string Key(string logicalName, Guid id) => logicalName + ":" + id;

        public Guid Create(Entity entity)
        {
            var id = entity.Id == Guid.Empty ? Guid.NewGuid() : entity.Id;
            var clone = Clone(entity);
            clone.Id = id;
            _store[Key(entity.LogicalName, id)] = clone;
            return id;
        }

        public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
        {
            if (!_store.TryGetValue(Key(entityName, id), out var e))
                throw new InvalidOperationException($"{entityName} {id} não encontrado.");
            return Clone(e);
        }

        public void Update(Entity entity)
        {
            var key = Key(entity.LogicalName, entity.Id);
            if (!_store.TryGetValue(key, out var existing))
            {
                existing = new Entity(entity.LogicalName, entity.Id);
                _store[key] = existing;
            }
            foreach (var attr in entity.Attributes) existing[attr.Key] = attr.Value;
        }

        public void Delete(string entityName, Guid id) => _store.Remove(Key(entityName, id));

        public EntityCollection RetrieveMultiple(QueryBase query)
        {
            var qe = query as QueryExpression;
            var name = qe?.EntityName ?? (query as QueryByAttribute)?.EntityName;
            IEnumerable<Entity> items = _store.Values.Where(e => name == null || e.LogicalName == name);
            if (qe?.Criteria != null) items = items.Where(e => MatchesFilter(e, qe.Criteria));
            if (qe != null && qe.Orders.Count > 0) items = ApplyOrders(items, qe.Orders);
            if (qe != null && qe.TopCount.HasValue) items = items.Take(qe.TopCount.Value);
            // (link-entity/joins não são simulados aqui — cobertos por org real / FakeXrmEasy.)
            return new EntityCollection(items.Select(Clone).ToList());
        }

        private static IEnumerable<Entity> ApplyOrders(IEnumerable<Entity> items, DataCollection<OrderExpression> orders)
        {
            IOrderedEnumerable<Entity> ordered = null;
            foreach (var o in orders)
            {
                object Key(Entity e) => Normalize(e.Contains(o.AttributeName) ? e[o.AttributeName] : null);
                ordered = ordered == null
                    ? (o.OrderType == OrderType.Ascending ? items.OrderBy(Key) : items.OrderByDescending(Key))
                    : (o.OrderType == OrderType.Ascending ? ordered.ThenBy(Key) : ordered.ThenByDescending(Key));
            }
            return ordered ?? items;
        }

        private static bool MatchesFilter(Entity e, FilterExpression filter)
        {
            var results = new List<bool>();
            foreach (var c in filter.Conditions) results.Add(MatchesCondition(e, c));
            foreach (var sub in filter.Filters) results.Add(MatchesFilter(e, sub));
            if (results.Count == 0) return true;
            return filter.FilterOperator == LogicalOperator.And ? results.All(x => x) : results.Any(x => x);
        }

        private static bool MatchesCondition(Entity e, ConditionExpression c)
        {
            var actual = Normalize(e.Contains(c.AttributeName) ? e[c.AttributeName] : null);
            object First() => Normalize(c.Values.Count > 0 ? c.Values[0] : null);
            switch (c.Operator)
            {
                case ConditionOperator.Equal: return Equals(actual, First());
                case ConditionOperator.NotEqual: return !Equals(actual, First());
                case ConditionOperator.In: return c.Values.Select(Normalize).Contains(actual);
                case ConditionOperator.ContainValues:
                {
                    var col = (e.Contains(c.AttributeName) ? e[c.AttributeName] : null) as OptionSetValueCollection;
                    if (col == null) return false;
                    var possui = col.Select(o => o.Value).ToList();
                    return c.Values.Select(v => Convert.ToInt32(Normalize(v))).Any(possui.Contains);
                }
                case ConditionOperator.Like: return Like(actual as string, c.Values.FirstOrDefault() as string);
                case ConditionOperator.GreaterThan: return Compare(actual, First()) > 0;
                case ConditionOperator.GreaterEqual: return Compare(actual, First()) >= 0;
                case ConditionOperator.LessThan: return Compare(actual, First()) < 0;
                case ConditionOperator.LessEqual: return Compare(actual, First()) <= 0;
                default: throw new NotSupportedException($"Operador {c.Operator} não simulado no fake.");
            }
        }

        // Desembrulha tipos do SDK para valores comparáveis.
        private static object Normalize(object v)
        {
            switch (v)
            {
                case OptionSetValue o: return o.Value;
                case Money m: return m.Value;
                case EntityReference r: return r.Id;
                case AliasedValue a: return Normalize(a.Value);
                default: return v;
            }
        }

        private static int Compare(object a, object b)
        {
            if (a == null || b == null) return -1;
            if (a is IComparable cmp && a.GetType() == b.GetType()) return cmp.CompareTo(b);
            return Convert.ToDecimal(a).CompareTo(Convert.ToDecimal(b));
        }

        private static bool Like(string value, string pattern)
        {
            if (value == null || pattern == null) return false;
            var regex = "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("%", ".*").Replace("_", ".") + "$";
            return System.Text.RegularExpressions.Regex.IsMatch(value, regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        public OrganizationResponse Execute(OrganizationRequest request) => throw new NotSupportedException();
        public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities) => throw new NotSupportedException();
        public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities) => throw new NotSupportedException();

        private static Entity Clone(Entity e)
        {
            var c = new Entity(e.LogicalName, e.Id);
            foreach (var attr in e.Attributes) c[attr.Key] = attr.Value;
            return c;
        }
    }

    /// <summary>IPluginExecutionContext configurável para testes.</summary>
    public sealed class FakePluginContext : IPluginExecutionContext
    {
        public int Stage { get; set; } = 20;
        public IPluginExecutionContext ParentContext { get; set; }

        public int Mode { get; set; }
        public int IsolationMode { get; set; }
        public int Depth { get; set; } = 1;
        public string MessageName { get; set; }
        public string PrimaryEntityName { get; set; }
        public Guid? RequestId { get; set; }
        public string SecondaryEntityName { get; set; }
        public ParameterCollection InputParameters { get; set; } = new ParameterCollection();
        public ParameterCollection OutputParameters { get; set; } = new ParameterCollection();
        public ParameterCollection SharedVariables { get; set; } = new ParameterCollection();
        public Guid UserId { get; set; }
        public Guid InitiatingUserId { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public Guid PrimaryEntityId { get; set; }
        public EntityImageCollection PreEntityImages { get; set; } = new EntityImageCollection();
        public EntityImageCollection PostEntityImages { get; set; } = new EntityImageCollection();
        public EntityReference OwningExtension { get; set; }
        public Guid CorrelationId { get; set; }
        public bool IsExecutingOffline { get; set; }
        public bool IsOfflinePlayback { get; set; }
        public bool IsInTransaction { get; set; }
        public Guid OperationId { get; set; }
        public DateTime OperationCreatedOn { get; set; }
    }
}
