using Template.Plugins.Common;
using Template.Plugins.Services;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 responsabilidade = 1 step: ao mudar o contato principal, propagar o vínculo.
    /// Plugin fino — a regra está em <see cref="IAccountService.PropagarContatoPrincipal"/>.
    /// </summary>
    public sealed class AtualizarRelacionamentoPlugin : PluginBase
    {
        public AtualizarRelacionamentoPlugin()
            => RegisterEvent(Stages.PostOperation, Messages.Update, Model.Account.EntityLogicalName, OnExecute);

        private void OnExecute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            account.Id = context.PluginContext.PrimaryEntityId; // garante o Id no Post-Operation
            context.Resolve<IAccountService>().PropagarContatoPrincipal(account);
        }
    }
}
