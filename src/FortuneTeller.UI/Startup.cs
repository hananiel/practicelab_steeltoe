using System;
using FortuneTeller.UI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;
using Steeltoe.Common.Discovery;
using Steeltoe.Common.Http.Discovery;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace FortuneTeller.UI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<FortuneServiceOptions>(Configuration.GetSection("fortuneService"));
         //   services.AddScoped<IDiscoveryClient, EurekaDiscoveryClient>();
            services.AddScoped<IFortuneService, FortuneServiceClient>();
            services.AddTransient<DiscoveryHttpMessageHandler>();
            services.AddHttpClient("fortunes", c => { c.BaseAddress = new Uri("http://fortuneService/api/fortunes/"); })
                .AddHttpMessageHandler<DiscoveryHttpMessageHandler>()
                .AddTypedClient<IFortuneService, FortuneServiceClient>();


           

            services.AddDistributedMemoryCache();
            services.AddSession();
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddOptions();
            services.ConfigureCloudFoundryOptions(Configuration);
            services.AddDiscoveryClient(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseCookiePolicy();

            app.UseMvc();
            app.UseDiscoveryClient();
        }
    }
}
