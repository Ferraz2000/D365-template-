using Microsoft.Xrm.Sdk;
using Template.Plugins.Common;
using Template.Plugins.Contatos;
namespace Template.Plugins.Contas
{
    /// <summary>
    /// Regra de negócio da conta. Use quando a regra cresce ou mexe em dados.
    /// As queries vêm dos repositórios por entidade; aqui só a decisão de negócio.
    /// </summary>
    public sealed class ContaServico
    {
        private readonly ContaRepositorio _contas;
        private readonly ContatoRepositorio _contatos;

        public ContaServico(ContaRepositorio contas, ContatoRepositorio contatos)
        {
            Guard.AgainstNull(contas, nameof(contas));
            Guard.AgainstNull(contatos, nameof(contatos));
            _contas = contas;
            _contatos = contatos;
        }

        /// <summary>Regra que consome uma query: barra conta com nome já existente.</summary>
        public void RejeitarSeNomeDuplicado(Conta conta)
        {
            Guard.AgainstNull(conta, nameof(conta));
            if (string.IsNullOrWhiteSpace(conta.Nome)) return;

            var existente = _contas.ObterPorNome(conta.Nome);
            if (existente != null && existente.Id != conta.Id)
                throw new InvalidPluginExecutionException($"Já existe uma conta com o nome '{conta.Nome}'.");
        }

        /// <summary>O contato principal da conta passa a tê-la como cliente-pai.</summary>
        public void PropagarContatoPrincipal(Conta conta)
        {
            Guard.AgainstNull(conta, nameof(conta));
            if (conta.ContatoPrincipalId == null) return;

            var contato = new Contato(conta.ContatoPrincipalId.Id)
            {
                ClientePaiId = new EntityReference(Conta.EntityLogicalName, conta.Id)
            };
            _contatos.Atualizar(contato);
        }
    }
}
