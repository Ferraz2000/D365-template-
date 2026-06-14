using System;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>Acesso a dados da tabela <c>contact</c>.</summary>
    public interface IContactRepository
    {
        Contact GetById(Guid id, params string[] columns);
        void Update(Contact contact);
    }
}
