using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Template.Plugins.Integracao;
using Xunit;

namespace Template.Plugins.Tests
{
    public class ClienteRestTests
    {
        private sealed class HandlerFalso : HttpMessageHandler
        {
            public HttpMethod Metodo;
            public string CorpoEnviado;

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
            {
                Metodo = request.Method;
                CorpoEnviado = request.Content == null ? null : await request.Content.ReadAsStringAsync();
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"ok\":true}") };
            }
        }

        [Fact]
        public void PostJson_envia_corpo_e_retorna_resposta()
        {
            var handler = new HandlerFalso();
            var cliente = new ClienteRest(new HttpClient(handler));

            var resposta = cliente.PostJson("https://exemplo.com/api", "{\"nome\":\"Contoso\"}");

            Assert.Equal(HttpMethod.Post, handler.Metodo);
            Assert.Contains("Contoso", handler.CorpoEnviado);
            Assert.Contains("ok", resposta);
        }

        [Fact]
        public void PostJson_lanca_em_erro_http()
        {
            var cliente = new ClienteRest(new HttpClient(new ErroHandler()));
            Assert.ThrowsAny<HttpRequestException>(() => cliente.PostJson("https://exemplo.com/api", "{}"));
        }

        private sealed class ErroHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
                => Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        }

        private sealed class FalhaDepoisOkHandler : HttpMessageHandler
        {
            private int _falhasRestantes;
            public int Chamadas;
            public FalhaDepoisOkHandler(int falhas) { _falhasRestantes = falhas; }
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
            {
                Chamadas++;
                if (_falhasRestantes-- > 0)
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("ok") });
            }
        }

        [Fact]
        public void PostJson_faz_retry_e_sucede_na_terceira()
        {
            var handler = new FalhaDepoisOkHandler(2);
            var cliente = new ClienteRest(new HttpClient(handler), maxTentativas: 3);

            var resp = cliente.PostJson("https://exemplo.com/api", "{}");

            Assert.Equal(3, handler.Chamadas);
            Assert.Contains("ok", resp);
        }
    }
}
