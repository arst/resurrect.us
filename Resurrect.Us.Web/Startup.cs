using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Resurrect.Us.Web.Service;
using Resurrect.Us.Data.Models;
using Resurrect.Us.Data.Services;
using Resurrect.Us.Semantic.Services;
using Resurrect.Us.Semantic.Semantic;

namespace Resurrect.Us.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddTransient<IWaybackService, WaybackService>();
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=resurrectus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            services.AddDbContext<ResurrectRecordsContext>(options => options.UseSqlServer(connection));
            services.AddTransient<IUrlCheckerService, UrlCheckerService>();
            services.AddTransient<IResurrectRecordsStorageService, ResurrectRecordsStorageService>();
            services.AddTransient<IDOMProcessingService, DOMProcessingService>();
            services.AddTransient<IKeyPointsExtractorService, KeyPointsExtractorService>();
            services.AddTransient<ITextTokenizer, TextTokenizer>();
            services.AddTransient<IWordsFrequencyCounter, WordsFrequencyCounter>();
            services.AddTransient<ISemanticService, SemanticService>();
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

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "redirect", 
                    template:"{tinyUrl}",
                    defaults: new { controller = "Redirect", action = "Index" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
