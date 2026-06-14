using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Repositories;
using Template.Plugins.Services;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Contexto local de um plugin: agrega os serviços do pipeline, dá acesso ao Target/Images
    /// tipados e resolve dependências (composition root / IoC básico — sem framework de DI no sandbox).
    /// Vive só durante o Execute.
    /// </summary>
    public sealed class LocalPluginContext
    {
        public IPluginExecutionContext PluginContext { get; }
        public ITracingService Tracing { get; }

        /// <summary>Serviço no contexto do usuário que disparou a operação.</summary>
        public IOrganizationService UserService { get; }

        /// <summary>Serviço com privilégios de sistema (use com parcimônia).</summary>
        public IOrganizationService SystemService { get; }

        public LocalPluginContext(IServiceProvider serviceProvider)
        {
            Guard.AgainstNull(serviceProvider, nameof(serviceProvider));

            PluginContext = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            Tracing = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            var factory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            UserService = factory.CreateOrganizationService(PluginContext.UserId);
            SystemService = factory.CreateOrganizationService(null);
        }

        // ---- atalhos do pipeline ----
        public string MessageName => PluginContext.MessageName;
        public int Stage => PluginContext.Stage;
        public int Depth => PluginContext.Depth;

        /// <summary>A entidade-alvo (Target) do step, quando existir.</summary>
        public bool TryGetTarget(out Entity target)
        {
            if (PluginContext.InputParameters.TryGetValue("Target", out var value) && value is Entity entity)
            {
                target = entity;
                return true;
            }
            target = null;
            return false;
        }

        /// <summary>
        /// O Target como entidade tipada (early-bound). <c>ToEntity</c> compartilha o
        /// AttributeCollection, então alterar a entidade tipada reflete no Target (essencial em Pre-Operation).
        /// </summary>
        public bool TryGetTarget<T>(out T target) where T : Entity
        {
            if (TryGetTarget(out var entity))
            {
                target = entity.ToEntity<T>();
                return true;
            }
            target = null;
            return false;
        }

        public T GetPreImage<T>(string alias) where T : Entity =>
            PluginContext.PreEntityImages.TryGetValue(alias, out var img) ? img.ToEntity<T>() : null;

        public T GetPostImage<T>(string alias) where T : Entity =>
            PluginContext.PostEntityImages.TryGetValue(alias, out var img) ? img.ToEntity<T>() : null;

        public void Trace(string message) => Tracing?.Trace(message);

        /// <summary>
        /// Composition root (factory simples): plugins resolvem abstrações; serviços recebem seus
        /// repositórios por entidade. Sem framework de DI — evita conflitos no sandbox.
        /// </summary>
        public T Resolve<T>() where T : class
        {
            if (typeof(T) == typeof(IAccountRepository)) return (T)(object)new AccountRepository(UserService);
            if (typeof(T) == typeof(IContactRepository)) return (T)(object)new ContactRepository(UserService);
            if (typeof(T) == typeof(IAccountService)) return (T)(object)new AccountService(new ContactRepository(UserService));

            throw new InvalidOperationException($"Sem registro de IoC para {typeof(T).FullName}.");
        }
    }
}
