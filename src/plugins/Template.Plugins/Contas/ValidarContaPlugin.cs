using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;

namespace Template.Plugins.Contas
{
    /// <summary>
    /// **Pre-Validation**: valida a conta antes de gravar (roda fora da transação — ideal para barrar cedo).
    /// Registrar: Create/Update / Pre-Validation / account.
    /// </summary>
    public sealed class ValidarContaPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Conta>(out var conta)) return;

            if (conta.Contains(Conta.Fields.Nome) && string.IsNullOrWhiteSpace(conta.Nome))
                throw new InvalidPluginExecutionException("O nome da conta é obrigatório.");

            if (conta.Receita.HasValue && conta.Receita.Value < 0)
                throw new InvalidPluginExecutionException("A receita não pode ser negativa.");
        }
    }
}
