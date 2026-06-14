using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// **Post-Operation + PreImage**: registra a mudança do nome comparando o valor anterior
    /// (PreImage) com o novo (Target). Mostra a diferença pré/pós: no Post o registro já foi gravado,
    /// e a PreImage guarda como ele estava antes.
    /// Registrar em: message=Update, stage=Post-Operation, entity=account, filtro=name,
    /// com **PreImage** chamada "preimage" contendo o atributo 'name'.
    /// </summary>
    public sealed class RegistrarMudancaNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Model.Account>(out var account)) return;
            if (!account.Contains(Model.Account.Fields.Name)) return;

            var anterior = context.GetPreImage<Model.Account>("preimage");
            var nomeAntigo = anterior?.Name;
            var nomeNovo = account.Name;
            if (nomeAntigo == nomeNovo) return;

            context.Trace($"Nome alterado de '{nomeAntigo}' para '{nomeNovo}'.");
            // Ex.: criar uma task de auditoria, gravar histórico, etc.
        }
    }
}
