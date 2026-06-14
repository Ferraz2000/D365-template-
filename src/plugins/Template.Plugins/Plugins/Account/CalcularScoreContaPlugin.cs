using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Repositories;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// **Custom Message (Custom API / Action)** como gatilho: o step é registrado na mensagem
    /// <c>tpl_CalcularScoreConta</c> (não num Create/Update). Lê os InputParameters do contrato e
    /// devolve via OutputParameters.
    ///
    /// Contrato (definido no Custom API):
    ///   Input:  AccountId (EntityReference, obrigatório)
    ///   Output: Score (int)
    /// </summary>
    public sealed class CalcularScoreContaPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            var accountRef = context.GetInput<EntityReference>("AccountId");
            if (accountRef == null)
                throw new InvalidPluginExecutionException("Parâmetro 'AccountId' é obrigatório.");

            var account = new AccountRepository(context.UserService)
                .GetById(accountRef.Id, Model.Account.Fields.Revenue, Model.Account.Fields.NumberOfEmployees);

            context.SetOutput("Score", CalcularScore(account));
        }

        private static int CalcularScore(Model.Account account)
        {
            var score = 0;
            if (account.Revenue >= 1_000_000m) score += 50;
            if (account.NumberOfEmployees >= 100) score += 30;
            return score;
        }
    }
}
