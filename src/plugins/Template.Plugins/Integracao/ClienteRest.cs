using System.Net.Http;
using System.Text;
using Template.Plugins.Common;

namespace Template.Plugins.Integracao
{
    /// <summary>
    /// Cliente REST simples para integração HTTP (infra). O <see cref="HttpClient"/> é injetado,
    /// então dá para testar com um handler falso, sem rede.
    /// Use em plugins **assíncronos** (chamada externa em step síncrono prende a transação).
    /// </summary>
    public sealed class ClienteRest
    {
        private readonly HttpClient _http;

        public ClienteRest(HttpClient http)
        {
            Guard.AgainstNull(http, nameof(http));
            _http = http;
        }

        public string PostJson(string url, string json)
        {
            Guard.AgainstNullOrEmpty(url, nameof(url));
            using (var content = new StringContent(json ?? string.Empty, Encoding.UTF8, "application/json"))
            {
                var resposta = _http.PostAsync(url, content).GetAwaiter().GetResult();
                resposta.EnsureSuccessStatusCode();
                return resposta.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
        }
    }
}
