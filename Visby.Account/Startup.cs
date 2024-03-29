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
using Karenia.Visby.Account.Models;
using Microsoft.EntityFrameworkCore;
using IdentityServer4;
using IdentityServer4.Services;
using Karenia.Visby.Account.Services;

namespace Karenia.Visby.Account
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

            services.AddDbContext<AccountContext>(
                options => options.UseNpgsql("Host=visby_account-db_1;Username=root;Password=123456;Database=account")
            );

            services.BuildServiceProvider().GetService<AccountContext>().Database.Migrate();
            services.AddSingleton<ICorsPolicyService>(new DefaultCorsPolicyService(new LoggerFactory().CreateLogger<DefaultCorsPolicyService>())
            {
                AllowedOrigins = new[] { "*" },
                AllowAll = true
            });
            services.AddScoped<AccountService>();
            services.AddScoped<AccountStore>();
            services.AddControllers();
            services.AddIdentityServer().AddJwtBearerClientAuthentication()
                .AddDeveloperSigningCredential(filename: "signing/cert")
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryApiResources(Config.GetApiResources())
            .AddResourceOwnerValidator<AccountStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(policy =>
            {
                policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
            });

            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }


    }
}
