using System.Net.Http;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    class HostHttpClient : IHostHttpClient
    {
        private IHttpClientFactory _clientFactory;

        public HostHttpClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public HttpClient CreateHttpClient()
        {
            var httpClient = _clientFactory.CreateClient(Constants.HostingHttpClientName);
            return httpClient;
        }
    }
}
