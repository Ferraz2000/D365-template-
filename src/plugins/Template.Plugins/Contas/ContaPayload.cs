using System;
using System.Globalization;

namespace Template.Plugins.Contas
{
    /// <summary>Monta o payload de integração da conta. Puro e testável (sem rede).</summary>
    public static class ContaPayload
    {
        public static string Json(Conta conta, Guid id)
        {
            Guard(conta);
            var receita = conta.Receita?.ToString(CultureInfo.InvariantCulture) ?? "null";
            return $"{{\"id\":\"{id}\",\"nome\":{Texto(conta.Nome)},\"receita\":{receita}}}";
        }

        private static void Guard(Conta conta)
        {
            if (conta == null) throw new ArgumentNullException(nameof(conta));
        }

        private static string Texto(string valor) =>
            valor == null ? "null" : "\"" + valor.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }
}
