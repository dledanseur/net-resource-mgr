using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Authentication;
using Services.Authentication. Gitlab;
using NetUserMgtMvc.Shared;
using Tools.Configuration;
using Tools.HttpHandler;
using Tools.HttpHandler.Impl;
using Services.Data;
using Services.Services.UserService;
using Microsoft.EntityFrameworkCore;

namespace NetUserMgt
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables("RM_");
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();

            services.AddScoped<DbContext, EFDBContext>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IAuthenticationService, GitlabAuthenticationService>();

            services.AddSingleton<IRestClient, RestClient>();
            
            services.AddSingleton<Authentication>();

            
            Authentication.ConfigureServices(services, Configuration);

            services.AddSingleton<IConfig>(new Config("RM_"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            //Authentication.SharedInstance.Configure(app, Configuration);
           
	        app.UseMvc(routes =>
	        {
	            routes.MapRoute(
	                name: "default",
	                template: "{controller=Home}/{action=Index}/{id?}");
	        });

			
        }
    }
}
