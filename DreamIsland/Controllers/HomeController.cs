namespace DreamIsland.Controllers
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Caching.Memory;

    using DreamIsland.Services.Island;
    using DreamIsland.Services.Island.Models;

    using static WebConstants;

    public class HomeController : Controller
    {
        private readonly IIslandService islandService;
        private readonly IMemoryCache cache;

        public HomeController(IIslandService islandService, IMemoryCache cache)
        {
            this.islandService = islandService;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var latestThreeIslands = this.cache.Get<IEnumerable<LatestIslandsServiceModel>>(LatestIslandsCacheKey);

            if(latestThreeIslands == null)
            {
                latestThreeIslands = this.islandService.LatestIslands();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(LatestIslandsCacheKey, latestThreeIslands, cacheOptions);
            }          

            return View(latestThreeIslands);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Chat()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }
    }
}
