using Template.Plugins.Common;
using Template.Plugins.Contatos;
namespace Template.Plugins.Contas
{
    /// <summary>Post-Operation: propaga o contato principal. Registrar: Update / Post-Operation / account / filtro=primarycontactid.</summary>
    public sealed class AtualizarRelacionamentoPlugin : PluginBase
    {
        protected override void Execute(LocalPluginContext context)
        {
            if (!context.TryGetTarget<Conta>(out var conta)) return;
            conta.Id = context.PluginContext.PrimaryEntityId;

            var crm = context.UserService;
            var servico = new ContaServico(new ContaRepositorio(crm), new ContatoRepositorio(crm));
            servico.PropagarContatoPrincipal(conta);
        }
    }
}
