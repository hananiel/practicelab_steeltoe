using System;
using FortuneTeller.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FortuneTeller.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(policy: "fortunes.read")]
    public class FortunesController : ControllerBase
    {
        private readonly ILogger<FortunesController> _logger;
        private IFortuneRepository _fortunes;

        public FortunesController(ILogger<FortunesController> logger, IFortuneRepository fortunes)
        {
            _logger = logger;
            _fortunes = fortunes;
        }

        // GET: api/fortunes/all
        [HttpGet("all")]
        public async Task<List<Fortune>> AllFortunesAsync()
        {
            Console.WriteLine("getting fortunes");
            _logger?.LogTrace("AllFortunesAsync");
            var entities = await _fortunes.GetAllAsync();
            return entities
                    .Select(fortune => new Fortune { Id = fortune.Id, Text = fortune.Text })
                    .ToList();
        }

        // GET api/fortunes/random
        [HttpGet("random")]
        public async Task<Fortune> RandomFortuneAsync()
        {
            _logger?.LogTrace("RandomFortuneAsync");
            var fortuneEntity = await _fortunes.RandomFortuneAsync();
            return new Fortune { Id = fortuneEntity.Id, Text = fortuneEntity.Text };
        }
    }
}
