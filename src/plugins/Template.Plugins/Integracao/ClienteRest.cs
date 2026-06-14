using System;
using System.Net.Http;
using System.Text;
using Template.Plugins.Common;

namespace Template.Plugins.Integracao
{
    /// <summary>
    /// Cliente REST simples para integração HTTP (infra). O <see cref="HttpClient"/> é injetado,
    /// então dá para testar com um handler falso, sem rede. Faz **retry** em falha transitória.
    /// Use em plugins **assíncronos** (chamada externa em step síncrono prende a transação).
    /// </summary>
    public sealed class ClienteRest
    {
        private readonly HttpClient _http;
        private readonly int _maxTentativas;

        public ClienteRest(HttpClient http, int maxTentativas = 3)
        {
            Guard.AgainstNull(http, nameof(http));
            _http = http;
            _maxTentativas = Math.Max(1, maxTentativas);
        }

        public string PostJson(string url, string json)
        {
            Guard.AgainstNullOrEmpty(url, nameof(url));

            Exception ultimoErro = null;
            for (var tentativa = 1; tentativa <= _maxTentativas; tentativa++)
            {
                try
                {
                    using (var content = new StringContent(json ?? string.Empty, Encoding.UTF8, "application/json"))
                    {
                        var resposta = _http.PostAsync(url, content).GetAwaiter().GetResult();
                        resposta.EnsureSuccessStatusCode();
                        return resposta.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    }
                }
                catch (HttpRequestException ex)
                {
                    ultimoErro = ex; // transitório → tenta de novo
                }
            }
            throw new HttpRequestException($"Falha após {_maxTentativas} tentativa(s).", ultimoErro);
        }
    }
}
