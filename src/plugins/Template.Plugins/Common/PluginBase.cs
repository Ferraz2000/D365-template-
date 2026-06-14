using System;
using Microsoft.Xrm.Sdk;

namespace Template.Plugins.Common
{
    /// <summary>
    /// Base de todos os plugins. Cuida do boilerplate (contexto, tracing, tratamento de erro)
    /// para que cada plugin concreto faça UMA coisa só — como um método.
    /// </summary>
    public abstract class PluginBase : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            Guard.AgainstNull(serviceProvider, nameof(serviceProvider));
            var ctx = new LocalPluginContext(serviceProvider);

            try
            {
                ctx.Trace($"[{GetType().Name}] início — message={ctx.PluginContext.MessageName}, stage={ctx.PluginContext.Stage}");
                Execute(ctx);
                ctx.Trace($"[{GetType().Name}] fim");
            }
            catch (InvalidPluginExecutionException)
            {
                throw; // erro de negócio já formatado — sobe como está
            }
            catch (Exception ex)
            {
                ctx.Trace($"[{GetType().Name}] erro: {ex}");
                throw new InvalidPluginExecutionException($"[{GetType().Name}] falhou: {ex.Message}", ex);
            }
        }

        /// <summary>A única responsabilidade do plugin. Implemente a regra aqui.</summary>
        protected abstract void Execute(LocalPluginContext context);
    }
}
