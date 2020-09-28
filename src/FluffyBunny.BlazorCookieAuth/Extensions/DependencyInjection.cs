using FluffyBunny.BlazorCookieAuth.Services;
using FluffyBunny.BlazorCookieAuth.Services.Default;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace FluffyBunny.BlazorCookieAuth
{
    public static class DependencyInjection
    {
        public static WebAssemblyHostBuilder AddBlazorCookieAuth(this WebAssemblyHostBuilder builder)
        {
            var baseAddress = builder.HostEnvironment.BaseAddress;
            var uri = new Uri(baseAddress);
            baseAddress = $"{uri.Scheme}://{uri.Authority}";

            HostAccessor hostAccessor = new HostAccessor
            {
                HostEnvironment = builder.HostEnvironment,
                BaseAddress = new Uri(baseAddress)
            };
            builder.Services.AddSingleton<IHostAccessor>(hostAccessor);

            builder.Services.AddTransient<AccountHelper>();
            builder.Services.AddTransient<HostAuthorizedHandler>();
            builder.Services.AddTransient<ExternalWatcherDelegatingHandler>();
            

            builder.Services.AddHttpClient(Constants.HostingHttpClientName,
             client => {
                client.BaseAddress = new Uri(baseAddress);
             }).AddHttpMessageHandler<HostAuthorizedHandler>();

            builder.Services.AddTransient<HttpClient>(sp => {
                var authorizedHandler = sp.GetRequiredService<HostAuthorizedHandler>();
                return new HttpClient(authorizedHandler)
                {
                    BaseAddress = new Uri(baseAddress)
                };
            });
            builder.Services.AddSingleton<IHostHttpClient, HostHttpClient>();
            builder.Services.AddSingleton<IAuthStatusService, AuthStatusService>();

            return builder;
        }
        public static WebAssemblyHostBuilder AddAuthPingServiceTimerService(this WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSingleton<HostAuthStatusTimerService>();
            builder.Services.AddSingleton<IHostAuthHandlerHook>(sp =>
            {
                return sp.GetRequiredService<HostAuthStatusTimerService>();
            });
            builder.Services.AddSingleton<KeepAliveTimerService>();
            builder.Services.AddSingleton<IExternalTrafficHandlerHook>(sp =>
            {
                return sp.GetRequiredService<KeepAliveTimerService>();
            });

            return builder;
        }
    }
}
