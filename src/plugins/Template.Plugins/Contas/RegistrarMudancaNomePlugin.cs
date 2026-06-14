using Template.Plugins.Common;

namespace Template.Plugins.Contas
{
    /// <summary>
    /// Post-Operation + PreImage: compara o nome anterior (PreImage) com o novo (Target).
    /// Registrar: Update / Post-Operation / account / filtro=name, com PreImage "preimage" (name).
    /// </summary>
    public sealed class RegistrarMudancaNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Conta>(out var conta)) return;
            if (!conta.Contains(Conta.Fields.Nome)) return;

            var anterior = context.GetPreImage<Conta>("preimage");
            var nomeAntigo = anterior?.Nome;
            var nomeNovo = conta.Nome;
            if (nomeAntigo == nomeNovo) return;

            context.Trace($"Nome alterado de '{nomeAntigo}' para '{nomeNovo}'.");
        }
    }
}
