using FortuneTeller.Service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pivotal.Discovery.Client;

namespace FortuneTeller.Service
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
          //  new Steeltoe.Discovery.Eureka.DiscoveryClient()
            services
                .AddDbContext<FortuneContext>(options=> options.UseInMemoryDatabase("my_database"))
                .AddScoped<IFortuneRepository,FortuneRepository>()
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDiscoveryClient(Configuration);
            
          
             
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDiscoveryClient();

            app.UseMvc();
        }

        
    }
}
