using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluffyBunny.BlazorCookieAuth.Services.Default
{
    public class KeepAliveTimerService :
          IExternalTrafficHandlerHook
    {
        private bool KeepAlive { get; set; }
        private Timer _timerKeepAlive;
        private IAuthStatusService _authPingService;
        private IServiceProvider _serviceProvider;

        public KeepAliveTimerService(IServiceProvider serviceProvider)
        {
            // TODO.  Blazor is crashing when I inject IAuthStatusService here.
            // Works just fine when I request it later using GetRequiredService
            _serviceProvider = serviceProvider;
            KeepAlive = false;
            _timerKeepAlive = new Timer(async _ =>
            {

                // start a 5 second keep alive timer
                // This is really here for when I have api calls going to other services 
                // Those calls will not slide out the host, but this will call the host for us.
                if (KeepAlive)
                {
                    KeepAlive = false;
                    if (_authPingService == null)
                    {
                        _authPingService = _serviceProvider.GetRequiredService<IAuthStatusService>();
                    }
                    await _authPingService.CheckAsync();
                }
            }, null, (5) * 1000, (5) * 1000);
        }

        public async Task OnTrafficAsync()
        {
            KeepAlive = true;
        }
    }
}
