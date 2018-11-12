using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Steeltoe.Common.Discovery;

namespace FortuneTeller.UI.Services
{
    public class FortuneServiceClient : IFortuneService
    {
        private readonly ILogger<FortuneServiceClient> _logger;
        private IOptions<FortuneServiceOptions> _config;
       // private HttpClient _httpClient;
        private DiscoveryHttpClientHandler _handler;

        private FortuneServiceOptions Config
        {
            get
            {
                return _config.Value;
            }
        }

        public FortuneServiceClient(
            IDiscoveryClient client,
            IOptions<FortuneServiceOptions> config, 
            ILogger<FortuneServiceClient> logger
         )
        {
            _logger = logger;
            _config = config;
           // _httpClient = httpClient;
            _handler = new DiscoveryHttpClientHandler(client);
            
            Console.WriteLine(client);

        }

        public async Task<List<Fortune>> AllFortunesAsync()
        {
           //Console.WriteLine(Config.AllFortunesURL);
            var httpClient = new HttpClient(_handler, false);
            var content =  await httpClient.GetStringAsync("http://fortuneService/api/fortunes/all" );
//           
            return JsonConvert.DeserializeObject<List<Fortune>>(content);
           // return new List<Fortune> {new Fortune() {Id = 1, Text = "testing"}};
         
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            return (await AllFortunesAsync())[0];
        }
    }
}