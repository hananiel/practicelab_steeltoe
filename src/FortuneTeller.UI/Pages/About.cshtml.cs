﻿using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace FortuneTeller.UI.Pages
{
    public class AboutModel : PageModel
    {
        public string Message { get; set; }
        
        private CloudFoundryServicesOptions Services { get; set; }

        public AboutModel(IOptions<CloudFoundryServicesOptions> servOptions)
        {
            Services = servOptions.Value;
        }
        public void OnGet()
        {
            Console.WriteLine("services : "+Services.ServicesList.Count);
            foreach (var service in Services.ServicesList)
            {
                ViewData[service.Name] = service.Name;
                ViewData[service.Plan] = service.Plan;
                Console.WriteLine("foo");
                Message += service.Name +":";
                Console.WriteLine(service.Plan);
            }
            Message = "Your Not application description page.";
        }
    }
}
