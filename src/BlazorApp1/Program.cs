using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluffyBunny.BlazorCookieAuth;
using BlazorApp1.Services;
using FluffyBunny.BlazorCookieAuth.Services;

namespace BlazorApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.AddBlazorCookieAuth();
            builder.AddAuthPingServiceTimerService();

            var baseAddress = builder.HostEnvironment.BaseAddress;
            var uri = new Uri(baseAddress);
            baseAddress = $"{uri.Scheme}://{uri.Authority}";

            builder.Services.AddHttpClient("external",
            client => {
                client.BaseAddress = new Uri(baseAddress);
            }).AddHttpMessageHandler<ExternalWatcherDelegatingHandler>();
            builder.Services.AddSingleton<ISimpleService, SimpleService>();
            builder.Services.AddSingleton<IFakeTokenFetchService, FakeTokenFetchService>();
            builder.Services.AddSingleton<IClientMemoryCache, ClientMemoryCache>();
            builder.Services.AddSingleton<IAuthHandlerStateSink, MyAuthHandlerStateSink>();
 

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}
