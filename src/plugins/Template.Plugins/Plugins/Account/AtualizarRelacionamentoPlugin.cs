using Template.Plugins.Common;
using Template.Plugins.Repositories;
using Template.Plugins.Services;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// 1 plugin = 1 step: ao mudar o contato principal, propagar o vínculo.
    /// Registrar em: message=Update, stage=Post-Operation, entity=account, filtro=primarycontactid.
    /// Tem regra + acesso a dados → delega a um service (montado com `new`, sem mágica de DI).
    /// </summary>
    public sealed class AtualizarRelacionamentoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            account.Id = context.PluginContext.PrimaryEntityId; // garante o Id no Post-Operation

            var crm = context.UserService;
            var service = new AccountService(new AccountRepository(crm), new ContactRepository(crm));
            service.PropagarContatoPrincipal(account);
        }
    }
}
