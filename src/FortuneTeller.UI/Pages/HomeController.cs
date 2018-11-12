using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace FortuneTeller.UI.Pages
{
    public class HomeController : Controller
    {
        private CloudFoundryServicesOptions Services { get; set; }

        public HomeController(IOptions<CloudFoundryServicesOptions> servOptions)
        {
            Services = servOptions.Value;
        }
        // GET
        public IActionResult About()
        {
            foreach (var service in Services.ServicesList)
            {
                ViewData[service.Name] = service.Name;
                ViewData[service.Plan] = service.Plan;
                Console.WriteLine(service.Name);
                Console.WriteLine(service.Plan);
            }
            
            return View();
        }
    }
}