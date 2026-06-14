using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Model;
using Template.Plugins.Repositories;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// Responsabilidade única: ao mudar o contato principal da conta, propaga o vínculo.
    /// Usa IRepository (acesso a dados via abstração — nunca IOrganizationService cru) e
    /// entidades tipadas (early-bound).
    /// Registro: message=Update, stage=Post-Operation, entity=account, filtro=primarycontactid.
    /// </summary>
    public sealed class AtualizarRelacionamentoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            if (account.PrimaryContactId == null) return;

            var repo = context.Resolve<IRepository>();

            // Refletir a conta como "parent" no contato principal.
            var contato = new Contact(account.PrimaryContactId.Id)
            {
                ParentCustomerId = new EntityReference(Model.Account.EntityLogicalName, context.PluginContext.PrimaryEntityId)
            };
            repo.Update(contato);
            context.Trace("Relacionamento conta↔contato atualizado.");
        }
    }
}
