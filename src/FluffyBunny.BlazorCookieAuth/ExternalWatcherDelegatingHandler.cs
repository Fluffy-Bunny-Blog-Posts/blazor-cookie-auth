using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluffyBunny.BlazorCookieAuth.Services;
using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace FluffyBunny.BlazorCookieAuth
{
    public class ExternalWatcherDelegatingHandler : DelegatingHandler
    {
        private readonly IExternalTrafficHandlerHook _authHandlerHook;
        public ExternalWatcherDelegatingHandler(AccountHelper accountHelper,
            IServiceProvider serviceProvider,
            IEnumerable<IAuthHandlerStateSink> authHandlerStateSinks)
        {
            _authHandlerHook = serviceProvider.GetService<IExternalTrafficHandlerHook>();
        }
         
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage responseMessage = await base.SendAsync(request, cancellationToken);
            if (responseMessage.IsSuccessStatusCode)
            {
                if (_authHandlerHook != null)
                {
                    await _authHandlerHook.OnTrafficAsync();
                }
            }
            return responseMessage;
        }
    }
}
