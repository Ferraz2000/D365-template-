using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Repositories;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Contexto local de um plugin: agrega os serviços do pipeline e resolve
    /// dependências (IoC básico). Vive só durante o Execute — sem estado entre execuções.
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

        public Entity GetPreImage(string alias) =>
            PluginContext.PreEntityImages.TryGetValue(alias, out var img) ? img : null;

        public Entity GetPostImage(string alias) =>
            PluginContext.PostEntityImages.TryGetValue(alias, out var img) ? img : null;

        public void Trace(string message) => Tracing?.Trace(message);

        /// <summary>
        /// IoC básico (Clean): plugins resolvem abstrações, nunca instanciam acesso a dados direto.
        /// Mantido simples de propósito — registre novas dependências aqui.
        /// </summary>
        public T Resolve<T>() where T : class
        {
            if (typeof(T) == typeof(IRepository))
                return (T)(object)new EntityRepository(UserService);

            throw new InvalidOperationException($"Sem registro de IoC para {typeof(T).FullName}.");
        }
    }
}
