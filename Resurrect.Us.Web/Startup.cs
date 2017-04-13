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
using Resurrect.Us.Web.Service.Wrappers;
using Serilog;
using System.IO;

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
            services.AddDbContext<ResurrectRecordsContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("ResurrectRecordsDatabase")));
            services.AddTransient<IUrlCheckerService, UrlCheckerService>();
            services.AddTransient<IResurrectRecordsStorageService, ResurrectRecordsStorageService>();
            services.AddTransient<IDOMProcessingService, DOMProcessingService>();
            services.AddTransient<IKeyPointsExtractorService, KeyPointsExtractorService>();
            services.AddTransient<ITextTokenizer, TextTokenizer>();
            services.AddTransient<IWordsFrequencyCounter, WordsFrequencyCounter>();
            services.AddTransient<ISemanticService, SemanticService>();
            services.AddTransient<IHashStrategy, Base32HashGenerationStrategy>();
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<IUrlShortenerService, UrlShortenerService>();
            services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.MSSqlServer(Configuration.GetConnectionString("ResurrectRecordsDatabase"), "logs")
                .CreateLogger();
            loggerFactory.AddSerilog();

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
