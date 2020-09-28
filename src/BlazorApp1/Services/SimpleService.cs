using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public class SimpleService : ISimpleService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public SimpleService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("external");
        }

        public async Task PingAsync()
        {
            await _httpClient.GetAsync("api/Simple/ping");
        }
    }
}
