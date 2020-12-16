using System;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OIDC_demo_API.Authorisation;

namespace OIDC_demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options => 
                {
                    options.Authority = "https://localhost:44318";
                    options.ApiName = "demoapi";
                    options.ApiSecret = "secret"; ;
                    // options.EnableCaching = true;
                    // options.CacheDuration = TimeSpan.FromMinutes(5);
                });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsOldEnough", builder =>
                {
                    builder.RequireAuthenticatedUser();
                    builder.AddRequirements(new IsOldEnough(30));
                });
            });

            services.AddScoped<IAuthorizationHandler, IsOldEnoughHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
