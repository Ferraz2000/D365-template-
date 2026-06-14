using Template.Plugins.Common;
using Template.Plugins.Services;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 responsabilidade = 1 step: normalizar o nome da conta.
    /// O plugin só orquestra (extrai o Target e delega ao service). Sem regra de negócio aqui.
    /// </summary>
    public sealed class AtualizarNomePlugin : PluginBase
    {
        public AtualizarNomePlugin()
            => RegisterEvent(Stages.PreOperation, Messages.Update, Model.Account.EntityLogicalName, OnExecute);

        private void OnExecute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            context.Resolve<IAccountService>().NormalizarNome(account);
        }
    }
}
