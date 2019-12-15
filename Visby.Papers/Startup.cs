using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Karenia.Visby.Papers.Models;
using Microsoft.EntityFrameworkCore;
using IdentityServer4;
using IdentityServer4.Services;
using Karenia.Visby.Papers.Services;
using Microsoft.IdentityModel.Logging;
namespace Karenia.Visby.Papers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionEnvironment =
                Environment.GetEnvironmentVariable(Configuration.GetConnectionString("ConnectionEnvironment"));

            services.AddDbContext<PaperContext>(
                options => options.UseNpgsql(
                    connectionEnvironment
                )
            );
            services.AddAuthentication("test").AddIdentityServerAuthentication("test", option =>
            {//TODO change into real ip
                option.Authority = "visby-account";
                //option.Audience = "api1";
                //option.MetadataAddress = "visby_visby-account_1" + "/.well-known/openid-configuration";
                option.ApiName = "api1";
                option.ApiSecret = "client";
                option.RequireHttpsMetadata = false;

                //option.Configuration = new Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration();
            }
           );

            services.AddAuthorization(option =>
            {
                option.AddPolicy(
                    "professorApi", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("Role", "professor");
                    });
                option.AddPolicy(
                    "userProfileApi", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("Role", "userProfile");
                    }
                );
                option.AddPolicy(
                    "adminApi", policy =>
                    {
                        policy.AddAuthenticationSchemes("test");
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("Role", "admin");
                    }
                );
            }
            );

            services.AddSingleton<PaperService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            IdentityModelEventSource.ShowPII = true;
            app.UseRouting();
            app.UseAuthentication();

            IdentityModelEventSource.ShowPII = true;
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}