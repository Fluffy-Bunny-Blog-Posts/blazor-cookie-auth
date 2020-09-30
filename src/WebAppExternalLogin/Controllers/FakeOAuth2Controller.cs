using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebAppExternalLogin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class FakeOAuth2Controller : ControllerBase
    {
        string GuidS => Guid.NewGuid().ToString();
        private readonly ILogger<FakeOAuth2Controller> _logger;

        public FakeOAuth2Controller(ILogger<FakeOAuth2Controller> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [Authorize]
        [Route("fake-bearer-token")]
        public async Task<object> GetFakeBearerTokenAsync()
        {
            var root = $"{Request.Scheme}://{Request.Host}/api/FakeOAuth2";
            return new
            {
                authority = root,
                access_token = GuidS,
                expires_in = 15,
                token_type = "Bearer",
                refresh_token = GuidS,
                client_id = GuidS,
                token_endpoint = $"{root}/connect/token"
            };
        }
        [HttpGet]
        [Route(".well-known/openid-configuration/jwks")]
        public async Task<object> GetJwksDocument()
        {
            return new { };

        }
        [HttpGet]
        [Route(".well-known/openid-configuration")]
        public async Task<object> GetDiscoveryDocumentAsync()
        {
            var root = $"{Request.Scheme}://{Request.Host}/api/FakeOAuth2";
            var result = new
            {
                issuer = root,
                jwks_uri = $"{root}/.well-known/openid-configuration/jwks",
                authorization_endpoint = $"{root}/connect/authorize",
                token_endpoint = $"{root}/connect/token",
                userinfo_endpoint = $"{root}/connect/userinfo",
                end_session_endpoint = $"{root}/connect/endsession",
                check_session_iframe = $"{root}/connect/checksession",
                revocation_endpoint = $"{root}/connect/revocation",
                introspection_endpoint = $"{root}/connect/introspect",
                device_authorization_endpoint = $"{root}/connect/deviceauthorization"

            };

            return result;
        }
        [HttpPost]
        [Route("connect/token")]
        public async Task<object> PostTokenAsync()
        {
            try
            {
                var form = (await HttpContext.Request.ReadFormAsync()).AsNameValueCollection();
                var grantType = form.Get(OidcConstants.TokenRequest.GrantType);
                switch (grantType)
                {
                    case OidcConstants.GrantTypes.RefreshToken:
                        return await HandleRefreshTokenAsync(form);
                }
                return new NotFoundObjectResult(null);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }


        }

        private async Task<object> HandleRefreshTokenAsync(System.Collections.Specialized.NameValueCollection form)
        {
            var refreshToken = form.Get(OidcConstants.TokenRequest.RefreshToken);
            return new
            {
                access_token = GuidS,
                expires_in = 15,
                token_type = "Bearer",
                refresh_token = GuidS
            };
        }
    }
}
