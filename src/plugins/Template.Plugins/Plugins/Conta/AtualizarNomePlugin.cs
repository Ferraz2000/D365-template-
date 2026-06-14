using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Conta
{
    /// <summary>Pre-Operation: normaliza o nome. Registrar: Update / Pre-Operation / account / filtro=name.</summary>
    public sealed class AtualizarNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Conta>(out var conta)) return;
            if (string.IsNullOrWhiteSpace(conta.Nome)) return;

            conta.Nome = conta.Nome.Trim();
            context.Trace("Nome da conta normalizado.");
        }
    }
}
