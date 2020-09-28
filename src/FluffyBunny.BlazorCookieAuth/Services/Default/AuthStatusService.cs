using System.Net.Http;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class AuthStatusService : IAuthStatusService
    {
        private readonly HttpClient _httpClient;

        public AuthStatusService(IHostHttpClient hostHttpClient)
        {
            _httpClient = hostHttpClient.CreateHttpClient();
        }

        public async Task CheckAsync()
        {
            await _httpClient.GetAsync("api/AuthStatus/check");
        }
    }
}
