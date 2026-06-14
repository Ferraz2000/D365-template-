using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Repositories;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// Responsabilidade única: ao mudar o contato principal da conta, propaga o vínculo.
    /// Usa IRepository (acesso a dados via abstração — nunca IOrganizationService cru).
    /// Registro: message=Update, stage=Post-Operation, entity=account, filtro=primarycontactid.
    /// </summary>
    public sealed class AtualizarRelacionamentoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget(out var target)) return;
            if (!target.Contains("primarycontactid")) return;

            var contato = target.GetAttributeValue<EntityReference>("primarycontactid");
            if (contato == null) return;

            var repo = context.Resolve<IRepository>();

            // Exemplo: refletir a conta como "parent" no contato principal.
            var update = new Entity(Tables.Contact, contato.Id)
            {
                ["parentcustomerid"] = new EntityReference(Tables.Account, context.PluginContext.PrimaryEntityId)
            };
            repo.Update(update);
            context.Trace("Relacionamento conta↔contato atualizado.");
        }
    }
}
