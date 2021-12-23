namespace DreamIsland.Controllers
{
    using System.Linq;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Models;
    using DreamIsland.Services.Island;

    public class HomeController : Controller
    {
        private readonly IIslandService islandService;

        public HomeController(IIslandService islandService)
        {
            this.islandService = islandService;
        }

        public IActionResult Index()
        {
            var firstThreeIslands = this.islandService.All().Take(3).ToList();

            return View(firstThreeIslands);
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
