using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WebAppExternalLogin.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAppExternalLogin.Extensions;
using Microsoft.AspNetCore.Routing;
using WebAppExternalLogin.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;

namespace WebAppExternalLogin
{
    public class Startup
    {
        public int CookieAuthExpirationSeconds { get; }
        public Startup(
            IConfiguration configuration,
            IHostEnvironment hostingEnvironment)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            CookieAuthExpirationSeconds = Convert.ToInt32(Configuration["CookieAuthExpirationSeconds"]);

        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthenticationPeekOptions>((options) =>
            {
                options.CookieAuthExpirationSeconds = CookieAuthExpirationSeconds;
            });
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                 builder =>
                 {

                     builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .AllowAnyOrigin();
                 }));
            // set forward header keys to be the same value as request's header keys
            // so that redirect URIs and other security policies work correctly.
            var aspNETCORE_FORWARDEDHEADERS_ENABLED = string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED"), "true", StringComparison.OrdinalIgnoreCase);
            if (aspNETCORE_FORWARDEDHEADERS_ENABLED)
            {
                //To forward the scheme from the proxy in non-IIS scenarios
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                    // Only loopback proxies are allowed by default.
                    // Clear that restriction because forwarders are enabled by explicit 
                    // configuration.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

            services.AddDbContext<ApplicationDbContext>(config =>
            {
                // for in memory database  
                config.UseInMemoryDatabase("InMemoryIdentityDatabase");
            });
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = $"{Configuration["applicationName"]}.AspNetCore.Identity.Application";
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        }
                        ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = (ctx) =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                            return Task.CompletedTask;
                        }
                        ctx.Response.Redirect(ctx.RedirectUri);
                        return Task.CompletedTask;
                    }
                };
                var authExpirationSeconds = Convert.ToInt32(CookieAuthExpirationSeconds);
                options.ExpireTimeSpan = new TimeSpan(0, 0, authExpirationSeconds);
            });

            services.AddAuthentication<IdentityUser>(Configuration);
            IMvcBuilder builder = services.AddRazorPages();
            if (HostingEnvironment.IsDevelopment())
            {
                builder.AddRazorRuntimeCompilation();
            }
            services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.Name = $"{Configuration["applicationName"]}.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(3600);
                options.Cookie.HttpOnly = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseForwardedHeaders();

            app.MapWhen(ctx => {
                return ctx.Request.Path.StartsWithSegments("/BlazorHost/BlazorApp1");
            }, config => {
                AddBlazorPathHosted(config, "BlazorHost/BlazorApp1");
            });

            app.MapWhen(ctx =>
            {
                if (
                ctx.Request.Path.StartsWithSegments("/BlazorHost/"))
                {
                    return false;
                }
                return true;
            }, config =>
            {
                app.UseStaticFiles();
                app.UseAuthentication();
                app.UseRouting();
                app.UseAuthorization();
                app.UseMiddleware<AuthenticationPeekMiddleware>();
                config.UseSession();
                app.UseEndpoints(endpoints =>
                {
                    MapBasicEndpoints(endpoints);
                });
            });

            
        }
        void AddBlazorPathHosted(IApplicationBuilder builder, string pattern)
        {
            builder.UseBlazorFrameworkFiles($"/{pattern}");
            builder.UseStaticFiles();
         
            builder.UseCookiePolicy();
            //  app.UseBlazorFrameworkFiles();

            builder.UseAuthentication();
            builder.UseRouting();
            builder.UseAuthorization();
            builder.UseMiddleware<AuthenticationPeekMiddleware>();

 
            builder.UseSession();
            builder.UseEndpoints(endpoints =>
            {
                MapBasicEndpoints(endpoints);
            });

        }
        private static void MapBasicEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
        }
 
    }
}
