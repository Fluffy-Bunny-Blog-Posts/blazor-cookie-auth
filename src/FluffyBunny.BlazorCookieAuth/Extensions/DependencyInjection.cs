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
            builder.Services.AddTransient<AuthorizedHandler>();

            builder.Services.AddHttpClient(Constants.HostingHttpClientName,
             client => {
                 baseAddress = $"{hostAccessor.BaseAddress.Scheme}://{hostAccessor.BaseAddress.Authority}";
                 client.BaseAddress = new Uri(baseAddress);
             }).AddHttpMessageHandler<AuthorizedHandler>();

            builder.Services.AddTransient<HttpClient>(sp => {
                var authorizedHandler = sp.GetRequiredService<AuthorizedHandler>();
                return new HttpClient(authorizedHandler)
                {
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
                };
            });
            builder.Services.AddSingleton<IHostHttpClient, HostHttpClient>();
            builder.Services.AddSingleton<IAuthPingService, AuthPingService>();

            return builder;
        }
        public static WebAssemblyHostBuilder AddAuthPingServiceTimerService(this WebAssemblyHostBuilder builder)
        {
            builder.Services.AddSingleton<AuthPingServiceTimerService>();
            builder.Services.AddSingleton<IAuthHandlerHook>(sp =>
            {
                return sp.GetRequiredService<AuthPingServiceTimerService>();
            });
            builder.Services.AddSingleton<IAuthPingServiceTimerService>(sp =>
            {
                return sp.GetRequiredService<AuthPingServiceTimerService>();
            });
            
            return builder;
        }
    }
}
