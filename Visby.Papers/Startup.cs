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
                        policy.RequireAuthenticatedUser();
                        policy.RequireClaim("Role", "admin");
                    }
                );
            }

            );
            services.AddAuthentication().AddIdentityServerAuthentication(option =>
            {//TODO change into real ip
                option.Authority = "localhost:6060";
                option.ApiName = "api1";
                option.ApiSecret = "client";
            }
            );
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}