using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace WebAppExternalLogin.Authorization
{
    public class AuthenticationPeekMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly AuthenticationPeekOptions _options;

        public AuthenticationPeekMiddleware(RequestDelegate next, IOptions<AuthenticationPeekOptions> options)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.User.Identity.IsAuthenticated)
            {
                context.Response.Headers.Add("x-authexpsec", (_options.CookieAuthExpirationSeconds + 60).ToString());
            }
            context.Response.Headers.Add("x-authenticated", (context.User.Identity.IsAuthenticated).ToString());

            await _next(context);
        }
    }
}
