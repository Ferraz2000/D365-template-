using Template.Plugins.Common;
using Template.Plugins.Repositories;

namespace Template.Plugins.Plugins.Conta
{
    /// <summary>
    /// **Anti-loop com Depth**: este plugin atualiza a própria conta — o que dispararia o Update
    /// de novo e causaria recursão infinita. O guard <c>context.Depth &gt; 1</c> corta a reentrada.
    /// Registrar: Update / Post-Operation / account.
    /// </summary>
    public sealed class RecalcularResumoContaPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (context.Depth > 1) return; // anti-loop: não reentrar

            if (!context.TryGetTarget<Model.Conta>(out var conta)) return;

            var atualizacao = new Model.Conta(context.PluginContext.PrimaryEntityId)
            {
                Resumo = $"{conta.Categoria} / {conta.Receita:C}"
            };
            new ContaRepositorio(context.UserService).Atualizar(atualizacao);
            context.Trace("Resumo recalculado.");
        }
    }
}
