using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Basics.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Basics
{
    public class Startup
    {     
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Grandmas.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });

            services.AddAuthorization(config =>
            {
                //var defaultAuthorizationBuilder = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthorizationBuilder
                //    .RequireAuthenticatedUser()
                //    .RequireClaim(ClaimTypes.DateOfBirth)
                //    .Build();

                //config.DefaultPolicy = defaultAuthPolicy;

                config.AddPolicy("Claim.DateOfBirth", policyBuilder =>
                {
                    //policyBuilder.RequireClaim(ClaimTypes.DateOfBirth);
                    //policyBuilder.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                    policyBuilder.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });
            });

            services.AddScoped<IAuthorizationHandler, CustomRequireClaimHandler>();

            services.AddControllersWithViews();
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
