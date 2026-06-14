using Template.Plugins.Common;
using Template.Plugins.Services;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 responsabilidade = 1 step: após criar a conta, enfileirar integração.
    /// Separado dos demais plugins da Account. Regra em <see cref="IAccountService.EnfileirarIntegracao"/>.
    /// </summary>
    public sealed class IntegracaoPlugin : PluginBase
    {
        public IntegracaoPlugin()
            => RegisterEvent(Stages.PostOperation, Messages.Create, Model.Account.EntityLogicalName, OnExecute);

        private void OnExecute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            account.Id = context.PluginContext.PrimaryEntityId;
            context.Resolve<IAccountService>().EnfileirarIntegracao(account);
        }
    }
}
