using Template.Plugins.Common;

namespace Template.Plugins.Plugins.Account
{
    /// <summary>
    /// Responsabilidade única: normaliza o nome da conta antes de gravar.
    /// Registro: message=Update, stage=Pre-Operation, entity=account, filtro=name.
    /// </summary>
    public sealed class AtualizarNomePlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget(out var target)) return;
            if (!target.Contains("name")) return;

            var nome = target.GetAttributeValue<string>("name");
            if (string.IsNullOrWhiteSpace(nome)) return;

            // Regra única: aparar espaços e padronizar. (Pre-Operation: alterar o Target basta.)
            target["name"] = nome.Trim();
            context.Trace("Nome da conta normalizado.");
        }
    }
}
