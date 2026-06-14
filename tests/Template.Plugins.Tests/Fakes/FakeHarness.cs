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

        public FakePluginContext Context(string message, int stage, string primaryEntityName = null) =>
            new FakePluginContext { MessageName = message, Stage = stage, PrimaryEntityName = primaryEntityName };

        public void Execute<TPlugin>(FakePluginContext context) where TPlugin : IPlugin, new()
        {
            var provider = new FakeServiceProvider(context, Tracing, new FakeOrganizationServiceFactory(Service));
            new TPlugin().Execute(provider);
        }
    }

    public sealed class FakeServiceProvider : IServiceProvider
    {
        private readonly IPluginExecutionContext _context;
        private readonly ITracingService _tracing;
        private readonly IOrganizationServiceFactory _factory;

        public FakeServiceProvider(IPluginExecutionContext context, ITracingService tracing, IOrganizationServiceFactory factory)
        {
            _context = context;
            _tracing = tracing;
            _factory = factory;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IPluginExecutionContext)) return _context;
            if (serviceType == typeof(ITracingService)) return _tracing;
            if (serviceType == typeof(IOrganizationServiceFactory)) return _factory;
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
            if (qe?.Criteria != null)
                items = items.Where(e => MatchesFilter(e, qe.Criteria));
            if (qe != null && qe.TopCount.HasValue)
                items = items.Take(qe.TopCount.Value);
            return new EntityCollection(items.Select(Clone).ToList());
        }

        // Filtro mínimo (Equal/NotEqual + subfiltros AND) — suficiente para testar queries dos repositórios.
        private static bool MatchesFilter(Entity e, FilterExpression filter)
        {
            foreach (var c in filter.Conditions)
            {
                var actual = e.Contains(c.AttributeName) ? e[c.AttributeName] : null;
                var expected = c.Values.Count > 0 ? c.Values[0] : null;
                var equal = Equals(actual, expected);
                if (c.Operator == ConditionOperator.Equal && !equal) return false;
                if (c.Operator == ConditionOperator.NotEqual && equal) return false;
            }
            foreach (var sub in filter.Filters)
                if (!MatchesFilter(e, sub)) return false;
            return true;
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
