using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xrm.Sdk;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Base de todos os plugins. Registra eventos (message + stage + entity) no construtor e,
    /// no Execute, dispara só o handler que casa com o step atual — mais o boilerplate de
    /// tracing e tratamento de erro. Convenção: **1 plugin registra 1 evento = 1 step**.
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        /// <summary>Config "unsecure" informada no registro do step (opcional).</summary>
        protected string UnsecureConfig { get; }

        /// <summary>Config "secure" informada no registro do step (opcional).</summary>
        protected string SecureConfig { get; }

        private readonly Collection<PluginEvent> _events = new Collection<PluginEvent>();

        protected PluginBase() { }

        protected PluginBase(string unsecureConfig, string secureConfig)
        {
            UnsecureConfig = unsecureConfig;
            SecureConfig = secureConfig;
        }

        /// <summary>
        /// Registra o evento que este plugin trata. <paramref name="entityLogicalName"/> vazio = qualquer entidade.
        /// </summary>
        protected void RegisterEvent(int stage, string message, string entityLogicalName, Action<LocalPluginContext> handler)
        {
            Guard.AgainstNull(handler, nameof(handler));
            _events.Add(new PluginEvent(stage, message, entityLogicalName, handler));
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            Guard.AgainstNull(serviceProvider, nameof(serviceProvider));
            var context = new LocalPluginContext(serviceProvider);
            var p = context.PluginContext;

            var handler = _events.FirstOrDefault(e =>
                e.Stage == p.Stage &&
                string.Equals(e.Message, p.MessageName, StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrEmpty(e.EntityLogicalName) ||
                 string.Equals(e.EntityLogicalName, p.PrimaryEntityName, StringComparison.OrdinalIgnoreCase)));

            if (handler == null) return; // step não casa com nenhum registro — nada a fazer

            try
            {
                context.Trace($"[{GetType().Name}] {p.MessageName}/{p.Stage}/{p.PrimaryEntityName} depth={p.Depth}");
                handler.Handler(context);
            }
            catch (InvalidPluginExecutionException)
            {
                throw; // erro de negócio já formatado — sobe como está
            }
            catch (Exception ex)
            {
                context.Trace($"[{GetType().Name}] erro: {ex}");
                throw new InvalidPluginExecutionException($"[{GetType().Name}] falhou: {ex.Message}", ex);
            }
        }

        private sealed class PluginEvent
        {
            public PluginEvent(int stage, string message, string entityLogicalName, Action<LocalPluginContext> handler)
            {
                Stage = stage;
                Message = message;
                EntityLogicalName = entityLogicalName;
                Handler = handler;
            }

            public int Stage { get; }
            public string Message { get; }
            public string EntityLogicalName { get; }
            public Action<LocalPluginContext> Handler { get; }
        }
    }
}
