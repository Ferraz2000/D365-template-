using System;
using Microsoft.Xrm.Sdk;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Base de todos os plugins. Cuida do boilerplate (contexto, tracing, tratamento de erro).
    /// Você só implementa <see cref="Execute(LocalPluginContext)"/> com a ação do step.
    /// O message/stage/entity é definido no **Plugin Registration** (e documentado no XML da classe).
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            Guard.AgainstNull(serviceProvider, nameof(serviceProvider));
            var context = new LocalPluginContext(serviceProvider);

            try
            {
                context.Trace($"[{GetType().Name}] início — {context.MessageName}/{context.Stage} depth={context.Depth}");
                Execute(context);
                context.Trace($"[{GetType().Name}] fim");
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

        /// <summary>A ação do plugin. Faça UMA coisa só.</summary>
        protected abstract void Execute(LocalPluginContext context);
    }
}
