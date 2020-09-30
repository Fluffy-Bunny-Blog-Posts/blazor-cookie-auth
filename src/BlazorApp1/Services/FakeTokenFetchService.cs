using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public class FakeTokenFetchService : IFakeTokenFetchService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _httpClient;

        public FakeTokenFetchService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = _clientFactory.CreateClient("external");
        }

        public async Task<TokenResponse> GetFakeBearerTokenAsync()
        {
            var response = await _httpClient.GetAsync("/api/FakeOAuth2/fake-bearer-token");
            if (response.IsSuccessStatusCode)
            {
                string stringData = await response.Content.ReadAsStringAsync();

                var user = JsonSerializer.Deserialize<TokenResponse>(stringData);

                return user;
            }
            return null;
        }
    }
}
