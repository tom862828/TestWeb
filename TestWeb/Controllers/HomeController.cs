using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TestWeb.Models;

namespace TestWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController));

        public HomeController()
        {
            //Do nothing
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
