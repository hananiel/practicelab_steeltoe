using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;
using FortuneTeller.Service.Models;

namespace FortuneTeller.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = await Task.FromResult(CreateWebHostBuilder(args).Build());

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
              
                    await SampleData.InitializeFortunesAsync(services);
                   
                    logger.LogInformation("Initialized Db");
                } 
                catch (Exception ex)
                {
                    
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
          
            host.Run();
        }

       

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) => 
                {
                    var env = hostingContext.HostingEnvironment;

                    config
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .UseIISIntegration()
                .UseUrls(new string[] { "http://*:5001" })
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                })
                .UseStartup<Startup>();

            if (args != null)
            {
                builder.UseConfiguration(new ConfigurationBuilder().AddCommandLine(args).Build());
            }

            return builder;
        }
    }
}
