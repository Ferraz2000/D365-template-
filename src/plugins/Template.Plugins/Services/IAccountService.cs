using Template.Plugins.Model;

namespace Template.Plugins.Services
{
    /// <summary>
    /// Regras de negócio da conta. **Aqui** mora a lógica — o plugin só orquestra.
    /// Depende de repositórios (abstrações), então é testável sem o pipeline do Dataverse.
    /// </summary>
    public interface IAccountService
    {
        void NormalizarNome(Account account);
        void PropagarContatoPrincipal(Account account);
        void EnfileirarIntegracao(Account account);
    }
}
