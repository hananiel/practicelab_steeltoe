using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace FortuneTeller.UI.Services
{
    public class FortuneServiceClient : IFortuneService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FortuneServiceClient> _logger;
        private IOptions<FortuneServiceOptions> _config;

        private FortuneServiceOptions Config
        {
            get
            {
                return _config.Value;
            }
        }

        private IHttpContextAccessor _reqContext;
        public FortuneServiceClient(
            HttpClient httpClient,
            IOptions<FortuneServiceOptions> config,
            ILogger<FortuneServiceClient> logger, IHttpContextAccessor context)
        {
            _httpClient = httpClient;
            _logger = logger;
            _config = config;
            _reqContext = context;
        }

        private async Task<HttpClient> GetAuthenticatedClient()
        {
            var token = await _reqContext.HttpContext.GetTokenAsync("access_token");
            _logger?.LogDebug("GetHttpClient found "+token);
            
           _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return _httpClient;
        }

        public async Task<List<Fortune>> AllFortunesAsync()
        {
            var authenticatedClient = await GetAuthenticatedClient();
            var response = await authenticatedClient.GetAsync(Config.AllFortunesURL);
            return await response.Content.ReadAsAsync<List<Fortune>>();
        }

        public async Task<Fortune> RandomFortuneAsync()
        {   
            var authenticatedClient = await GetAuthenticatedClient();
            var response = await authenticatedClient.GetAsync(Config.RandomFortuneURL);
            return await response.Content.ReadAsAsync<Fortune>();
        }
    }
}