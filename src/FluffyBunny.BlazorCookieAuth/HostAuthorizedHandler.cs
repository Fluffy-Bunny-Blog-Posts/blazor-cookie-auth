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
    public class HostAuthorizedHandler : DelegatingHandler
    {
        private readonly AccountHelper _accountHelper;
        private readonly IHostAuthHandlerHook _authHandlerHook;
        private readonly List<IAuthHandlerStateSink> _authHandlerStateSinks;

        public HostAuthorizedHandler(AccountHelper accountHelper, 
            IServiceProvider serviceProvider,
            IEnumerable<IAuthHandlerStateSink> authHandlerStateSinks)
        {
            _accountHelper = accountHelper;
            _authHandlerHook = serviceProvider.GetService<IHostAuthHandlerHook>();
            _authHandlerStateSinks = authHandlerStateSinks.ToList();
        }
        int? CheckForAuthSeconds(HttpResponseMessage responseMessage)
        {
            var query = from item in responseMessage.Headers
                        where item.Key == "x-authexpsec"
                        select item.Value;

            var authExpSec = query.FirstOrDefault();

            if (authExpSec != null)
            {
                var sec = authExpSec.FirstOrDefault();
                if (!string.IsNullOrEmpty(sec))
                {
                    return Convert.ToInt32(sec);
                }
            }
            return null;
        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage responseMessage = await base.SendAsync(request, cancellationToken);
            int? authSeconds = CheckForAuthSeconds(responseMessage);
            if (authSeconds == null)
            {
                // not authenicated
                foreach (var sink in _authHandlerStateSinks)
                {
                    await sink.OnAuthenticatedAsync(false);
                }
            }
            if (responseMessage.StatusCode == HttpStatusCode.Unauthorized ||
                responseMessage.StatusCode == HttpStatusCode.Forbidden)
            {
                // if server returned 401 Unauthorized, redirect to login page
                _accountHelper.SignIn();
                return null;
            }
            else
            {
                if (_authHandlerHook != null)
                {
                    var query = from item in responseMessage.Headers
                                where item.Key == "x-authexpsec"
                                select item.Value;

                    var authExpSec = query.FirstOrDefault();

                    if (authExpSec != null)
                    {
                        var sec = authExpSec.FirstOrDefault();
                        if (!string.IsNullOrEmpty(sec))
                        {
                            await _authHandlerHook.OnAuthorizedExpiresInAsync(Convert.ToInt32(sec));
                        }
                    }
                }
                return responseMessage;
            }
        }
    }
}
