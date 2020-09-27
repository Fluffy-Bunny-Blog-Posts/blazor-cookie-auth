using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class AuthPingServiceTimerService : IAuthPingServiceTimerService, IAuthHandlerHook
    {
        public AuthPingServiceTimerService(System.IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
         
        }


        private Timer _timer;
        private IAuthPingService _authPingService;
        private IServiceProvider _serviceProvider;

        public async Task OnAuthorizedCallAsync(long seconds)
        {
            if (_authPingService == null)
            {
                _authPingService = _serviceProvider.GetRequiredService<IAuthPingService>();
            }
            
            if (_timer != null)
            {
                _timer.Dispose();
            }
            _timer = new Timer(async _ =>
            {
                await _authPingService.PingAsync();
            }, null, seconds * 1000, seconds * 1000000);
             
        }
    }
}
