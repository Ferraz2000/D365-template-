using System;
using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
namespace Template.Plugins.Contatos
{
    /// <summary>Acesso a dados de <c>contact</c>.</summary>
    public sealed class ContatoRepositorio : RepositoryBase
    {
        public ContatoRepositorio(IOrganizationService service) : base(service) { }

        public Contato ObterPorId(Guid id, params string[] colunas) => RetrieveById<Contato>(id, colunas);

        public void Atualizar(Contato contato) => Update(contato);
    }
}
