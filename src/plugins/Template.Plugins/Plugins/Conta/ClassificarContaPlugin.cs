using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Conta
{
    /// <summary>Pre-Operation: classifica pela receita (Money → OptionSet). Registrar: Update / Pre-Operation / account / filtro=revenue.</summary>
    public sealed class ClassificarContaPlugin : PluginBase
    {
        private const decimal LimitePreferencial = 1_000_000m;

        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Conta>(out var conta)) return;
            if (conta.Receita == null) return;

            conta.Categoria = conta.Receita >= LimitePreferencial
                ? Model.CategoriaConta.ClientePreferencial
                : Model.CategoriaConta.Padrao;

            context.Trace($"Conta classificada como {conta.Categoria} (receita {conta.Receita:C}).");
        }
    }
}
