using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FortuneTeller.UI.Services
{
    public class FortuneServiceClient : IFortuneService
    {
        private readonly ILogger<FortuneServiceClient> _logger;
        private IOptions<FortuneServiceOptions> _config;
        private HttpClient _httpClient;

        private FortuneServiceOptions Config
        {
            get
            {
                return _config.Value;
            }
        }

        public FortuneServiceClient(
            
        HttpClient httpClient,
            IOptions<FortuneServiceOptions> config, 
            ILogger<FortuneServiceClient> logger)
        {
            _logger = logger;
            _config = config;
            _httpClient = httpClient;

        }

        public async Task<List<Fortune>> AllFortunesAsync()
        {
            Console.WriteLine(Config.AllFortunesURL);
            var content =  await _httpClient.GetStringAsync(Config.AllFortunesURL);
           
            return JsonConvert.DeserializeObject<List<Fortune>>(content);
        }

        public async Task<Fortune> RandomFortuneAsync()
        {
            return (await AllFortunesAsync())[0];
        }
    }
}