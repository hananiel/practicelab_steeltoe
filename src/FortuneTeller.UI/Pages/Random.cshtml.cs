using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Threading.Tasks;
using FortuneTeller.UI.Services;

namespace FortuneTeller.UI.Pages
{
    public class RandomModel : PageModel
    {
        public string Message { get; private set; } = "Hello from FortuneTellerUI!";
        private IFortuneService _fortuneService;

        public RandomModel(IFortuneService fortuneService)
        {
            _fortuneService = fortuneService;
        }

        public async Task OnGet()
        {
            var fortune = await _fortuneService.RandomFortuneAsync();
            Message = fortune.Text;
            
            HttpContext.Session.Set("MyFortune", Encoding.ASCII.GetBytes(fortune.Text));
        }
    }
}