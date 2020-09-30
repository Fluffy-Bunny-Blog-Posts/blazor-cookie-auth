using FluffyBunny.BlazorCookieAuth.Services;
using System.Threading.Tasks;

namespace BlazorApp1.Services
{
    public class MyAuthHandlerStateSink : IAuthHandlerStateSink
    {
        private IClientMemoryCache _clientMemoryCache;

        public MyAuthHandlerStateSink(IClientMemoryCache clientMemoryCache)
        {
            _clientMemoryCache = clientMemoryCache;

        }
        public async Task OnAuthenticatedAsync(bool authenticated)
        {
            if (!authenticated)
            {
                _clientMemoryCache.Clear();
            }
        }
    }
}
