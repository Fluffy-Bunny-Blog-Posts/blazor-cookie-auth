using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class HostAuthStatusTimerService : 
        IHostAuthHandlerHook
    {
        private Timer _timerExpiration;
        private IAuthStatusService _authPingService;
        private IServiceProvider _serviceProvider;

        public HostAuthStatusTimerService(IServiceProvider serviceProvider)
        {
            // TODO.  Blazor is crashing when I inject IAuthStatusService here.
            // Works just fine when I request it later using GetRequiredService
            _serviceProvider = serviceProvider;
        }

        public async Task OnAuthorizedExpiresInAsync(long seconds)
        {
            if (_authPingService == null)
            {
                _authPingService = _serviceProvider.GetRequiredService<IAuthStatusService>();
            }
            // Set this timer to trigger after we expire so that it auto triggers a login
            if (_timerExpiration != null)
            {
                _timerExpiration.Dispose();
            }
            _timerExpiration = new Timer(async _ =>
            {
                await _authPingService.CheckAsync();
            }, null, (seconds+5) * 1000, seconds * 1000000);
        }
    }
}
