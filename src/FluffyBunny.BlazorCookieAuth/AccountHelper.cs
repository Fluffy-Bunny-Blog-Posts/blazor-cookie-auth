using FluffyBunny.BlazorCookieAuth.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;

namespace FluffyBunny.BlazorCookieAuth
{
    public class AccountHelper
    {
        private const string LogInPath = "Identity/Account/Login";
        private const string LogOutPath = "Identity/Account/Logout";

        private readonly NavigationManager _navigation;
        private readonly IHostAccessor _hostAccessor;
        private readonly ILogger<AccountHelper> _logger;


        public AccountHelper(
            NavigationManager navigation,
            IHostAccessor hostAccessor,
            ILogger<AccountHelper> logger)
        {
            _navigation = navigation;
            _hostAccessor = hostAccessor;
            _logger = logger;
        }

        public void SignIn(string customReturnUrl = null)
        {
  
            var returnUrl = customReturnUrl != null ? _navigation.ToAbsoluteUri(customReturnUrl).ToString() : null;
            var encodedReturnUrl = Uri.EscapeDataString(returnUrl ?? new Uri(_navigation.Uri).PathAndQuery);
            var path = $"{LogInPath}?returnUrl={encodedReturnUrl}";
            var logInUrl = new Uri(_hostAccessor.BaseAddress, path);
            _navigation.NavigateTo(logInUrl.ToString(), true);
        }

        public void SignOut()
        {
            var logOutPath = new Uri(_hostAccessor.BaseAddress, $"{LogOutPath}?returnUrl=/");
            _navigation.NavigateTo(logOutPath.ToString(), true); ;
        }
    }
}
