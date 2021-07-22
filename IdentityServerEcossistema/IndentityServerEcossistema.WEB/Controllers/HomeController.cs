using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcClient.Models;
using System.Diagnostics;

namespace MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        
        [Authorize(Roles = "Admin")]        
        public IActionResult Privacy()
        {            
            return View();
        }

        [Authorize(Roles = "Admin,Tester")]
        public IActionResult Publico()
        {
            return View();
        }


        [Authorize(Roles = "Analista")]
        public IActionResult Analista()
        {
            return View();
        }

        [Authorize(Roles = "Tester")]
        public IActionResult Tester()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
