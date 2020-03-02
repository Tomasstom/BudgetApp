using BudgetApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BudgetApp.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { Message = TempData["Error"].ToString() });
        }
    }
}
