using System;
using Microsoft.Xrm.Sdk;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Contexto local de um plugin: agrega os serviços do pipeline e dá acesso ao Target/Images
    /// tipados. Vive só durante o Execute. (Dependências, quando precisar, são montadas com `new`
    /// no próprio plugin — sem framework de DI no sandbox.)
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

        /// <summary>A entidade-alvo (Target) do step, como entidade tipada (early-bound).</summary>
        public bool TryGetTarget<T>(out T target) where T : Entity
        {
            if (PluginContext.InputParameters.TryGetValue("Target", out var value) && value is Entity entity)
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

        // ---- mensagens customizadas (Custom API / Custom Action) ----
        public bool IsMessage(string message) =>
            string.Equals(MessageName, message, StringComparison.OrdinalIgnoreCase);

        /// <summary>Lê um parâmetro de entrada (do Target ou de um Custom API/Action).</summary>
        public T GetInput<T>(string name) =>
            PluginContext.InputParameters.TryGetValue(name, out var v) && v is T typed ? typed : default(T);

        /// <summary>Escreve um parâmetro de saída (resposta de um Custom API/Action).</summary>
        public void SetOutput(string name, object value) =>
            PluginContext.OutputParameters[name] = value;
    }
}
