using System.Net.Http;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class AuthPingService : IAuthPingService
    {
        private readonly HttpClient _httpClient;

        public AuthPingService(IHostHttpClient hostHttpClient)
        {
            _httpClient = hostHttpClient.CreateHttpClient();
        }

        public async Task PingAsync()
        {
            await _httpClient.GetAsync("api/AuthStatus/ping");
        }
    }
}
