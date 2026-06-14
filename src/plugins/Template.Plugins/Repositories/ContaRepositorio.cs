using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Template.Plugins.Model;

namespace Template.Plugins.Repositories
{
    /// <summary>
    /// Acesso a dados de <c>account</c>. **Toda query mora aqui.** Catálogo: igualdade, Like, In,
    /// Money, filtro composto, multi-select (ContainValues), link-entity (N:1) e N:N (Associate).
    /// </summary>
    public sealed class ContaRepositorio : RepositoryBase
    {
        public ContaRepositorio(IOrganizationService service) : base(service) { }

        public Guid Criar(Conta conta) => Create(conta);
        public void Atualizar(Conta conta) => Update(conta);
        public Conta ObterPorId(Guid id, params string[] colunas) => RetrieveById<Conta>(id, colunas);

        /// <summary>Igualdade.</summary>
        public Conta ObterPorNome(string nome)
        {
            var q = new QueryExpression(Conta.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Conta.Fields.Nome, Conta.Fields.ContatoPrincipalId),
                TopCount = 1
            };
            q.Criteria.AddCondition(Conta.Fields.Nome, ConditionOperator.Equal, nome);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).FirstOrDefault();
        }

        /// <summary>Like + ordenação.</summary>
        public IList<Conta> BuscarPorNome(string trecho)
        {
            var q = new QueryExpression(Conta.EntityLogicalName) { ColumnSet = new ColumnSet(Conta.Fields.Nome) };
            q.Criteria.AddCondition(Conta.Fields.Nome, ConditionOperator.Like, $"%{trecho}%");
            q.AddOrder(Conta.Fields.Nome, OrderType.Ascending);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).ToList();
        }

        /// <summary>OptionSet (enum) com In.</summary>
        public IList<Conta> ListarPorCategoria(params CategoriaConta[] categorias)
        {
            var valores = categorias.Select(c => (object)(int)c).ToArray();
            var q = new QueryExpression(Conta.EntityLogicalName) { ColumnSet = new ColumnSet(Conta.Fields.Nome, Conta.Fields.Categoria) };
            q.Criteria.AddCondition(Conta.Fields.Categoria, ConditionOperator.In, valores);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).ToList();
        }

        /// <summary>Money: receita ≥ mínimo, maiores primeiro.</summary>
        public IList<Conta> TopPorReceita(decimal minimo, int top)
        {
            var q = new QueryExpression(Conta.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Conta.Fields.Nome, Conta.Fields.Receita),
                TopCount = top
            };
            q.Criteria.AddCondition(Conta.Fields.Receita, ConditionOperator.GreaterEqual, minimo);
            q.AddOrder(Conta.Fields.Receita, OrderType.Descending);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).ToList();
        }

        /// <summary>Filtro composto: ativo E (preferencial OU com muitos funcionários).</summary>
        public IList<Conta> ListarAtivasPreferenciaisOuGrandes(int minFuncionarios)
        {
            var q = new QueryExpression(Conta.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(Conta.Fields.Nome, Conta.Fields.Categoria, Conta.Fields.NumeroDeFuncionarios)
            };
            q.Criteria.AddCondition(Conta.Fields.Estado, ConditionOperator.Equal, (int)EstadoConta.Ativo);
            var ou = new FilterExpression(LogicalOperator.Or);
            ou.AddCondition(Conta.Fields.Categoria, ConditionOperator.Equal, (int)CategoriaConta.ClientePreferencial);
            ou.AddCondition(Conta.Fields.NumeroDeFuncionarios, ConditionOperator.GreaterEqual, minFuncionarios);
            q.Criteria.AddFilter(ou);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).ToList();
        }

        /// <summary>Multi-select OptionSet: contas que oferecem o serviço (ContainValues).</summary>
        public IList<Conta> ComServico(Servico servico)
        {
            var q = new QueryExpression(Conta.EntityLogicalName) { ColumnSet = new ColumnSet(Conta.Fields.Nome, Conta.Fields.Servicos) };
            q.Criteria.AddCondition(Conta.Fields.Servicos, ConditionOperator.ContainValues, (object)(int)servico);
            return RetrieveMultiple(q).Entities.Select(e => e.ToEntity<Conta>()).ToList();
        }

        /// <summary>Relacionamento N:1 na query (link-entity + AliasedValue).</summary>
        public IDictionary<Guid, string> NomesDosContatosPrincipais()
        {
            var q = new QueryExpression(Conta.EntityLogicalName) { ColumnSet = new ColumnSet(Conta.Fields.Nome) };
            var link = q.AddLink(Contato.EntityLogicalName, Conta.Fields.ContatoPrincipalId, Contato.Fields.ContatoId, JoinOperator.LeftOuter);
            link.EntityAlias = "cp";
            link.Columns = new ColumnSet(Contato.Fields.NomeCompleto);
            return RetrieveMultiple(q).Entities.ToDictionary(
                e => e.Id,
                e => e.GetAttributeValue<AliasedValue>("cp." + Contato.Fields.NomeCompleto)?.Value as string);
        }

        /// <summary>Relacionamento N:N: associa contatos à conta.</summary>
        public void AssociarContatos(Guid contaId, string relacionamento, IEnumerable<Guid> contatoIds)
        {
            var refs = contatoIds.Select(id => new EntityReference(Contato.EntityLogicalName, id)).ToArray();
            Associate(Conta.EntityLogicalName, contaId, relacionamento, refs);
        }
    }
}
