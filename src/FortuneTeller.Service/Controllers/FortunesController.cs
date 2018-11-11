using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FortuneTeller.Service.Models;

namespace FortuneTeller.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortunesController : ControllerBase
    {
        private readonly ILogger<FortunesController> _logger;
        private readonly IFortuneRepository _repository;

        public FortunesController(ILogger<FortunesController> logger, IFortuneRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        // GET: api/fortunes/all
        [HttpGet("all")]
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            _logger?.LogTrace("AllFortunesAsync");
          
            var result =  _repository.GetAllAsync().Result.Select(x => new Fortune() {Id = x.Id, Text = x.Text}).ToList();
            return await Task.FromResult(result);
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public async Task<Fortune> RandomFortuneAsync()
        {
            _logger?.LogTrace("RandomFortuneAsync");
            return (await AllFortunesAsync())[0];
        }
    }
}
