using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 plugin = 1 step: classifica a conta pela receita (Money → OptionSet).
    /// Registrar em: message=Update, stage=Pre-Operation, entity=account, filtro=revenue.
    /// Regra simples sobre o Target → fica no plugin.
    /// </summary>
    public sealed class ClassificarContaPlugin : PluginBase
    {
        private const decimal LimitePreferencial = 1_000_000m;

        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            if (account.Revenue == null) return;

            account.Category = account.Revenue >= LimitePreferencial
                ? Model.AccountCategory.PreferredCustomer
                : Model.AccountCategory.Standard;

            context.Trace($"Conta classificada como {account.Category} (receita {account.Revenue:C}).");
        }
    }
}
