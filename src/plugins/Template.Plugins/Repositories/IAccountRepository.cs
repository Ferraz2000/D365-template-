using System;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Acesso a dados da tabela <c>account</c>. As queries da conta ficam aqui.</summary>
    public interface IAccountRepository
    {
        Guid Create(Account account);
        Account GetById(Guid id, params string[] columns);

        /// <summary>Exemplo de query no repositório (não no service/plugin).</summary>
        Account GetByName(string name);
    }
}
