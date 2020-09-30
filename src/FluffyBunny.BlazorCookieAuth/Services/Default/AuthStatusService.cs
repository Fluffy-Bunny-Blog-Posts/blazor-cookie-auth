using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class AuthStatusService : IAuthStatusService
    {
        private readonly ILogger<AuthStatusService> _logger;
        private readonly HttpClient _httpClient;

        public AuthStatusService(
            ILogger<AuthStatusService> logger,
            IHostHttpClient hostHttpClient)
        {
            _logger = logger;
            _httpClient = hostHttpClient.CreateHttpClient();
        }

        public async Task CheckAsync()
        {
            try
            {
                await _httpClient.GetAsync("api/AuthStatus/check");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
