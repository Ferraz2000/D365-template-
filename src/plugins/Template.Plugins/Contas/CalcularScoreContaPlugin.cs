using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
namespace Template.Plugins.Contas
{
    /// <summary>
    /// Custom Message (Custom API/Action) <c>tpl_CalcularScoreConta</c> como gatilho.
    /// Input: AccountId (EntityReference). Output: Score (int).
    /// </summary>
    public sealed class CalcularScoreContaPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            var contaRef = context.GetInput<EntityReference>("AccountId");
            if (contaRef == null)
                throw new InvalidPluginExecutionException("Parâmetro 'AccountId' é obrigatório.");

            var conta = new ContaRepositorio(context.UserService)
                .ObterPorId(contaRef.Id, Conta.Fields.Receita, Conta.Fields.NumeroDeFuncionarios);

            context.SetOutput("Score", CalcularScore(conta));
        }

        private static int CalcularScore(Conta conta)
        {
            var score = 0;
            if (conta.Receita >= 1_000_000m) score += 50;
            if (conta.NumeroDeFuncionarios >= 100) score += 30;
            return score;
        }
    }
}
