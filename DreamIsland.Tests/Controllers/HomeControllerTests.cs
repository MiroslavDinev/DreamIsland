namespace DreamIsland.Tests.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Services.Island.Models;

    using static Data.Islands;
    using static WebConstants;

    public class HomeControllerTests
    {
        [Fact]
        public void PrivacyShouldReturnView()
        {
             MyController<HomeController>
                .Calling(c => c.Privacy())
                .ShouldReturn()
                .View();
        }

        [Fact]
        
        public void ChatShouldReturnView()
        {
            MyController<HomeController>
                .Calling(c => c.Chat())
                .ShouldReturn()
                .View();
        }

        [Fact]

        public void IndexShouldReturnCorrectViewWithModel()
        {
            MyController<HomeController>
                .Instance(controller => controller
                .WithData(TenPublicIslands()))
                .Calling(c => c.Index())
                .ShouldHave()
                .MemoryCache(cache => cache
                .ContainingEntry(entry => entry
                .WithKey(LatestIslandsCacheKey)
                .WithAbsoluteExpirationRelativeToNow(TimeSpan.FromMinutes(15))
                .WithValueOfType<List<LatestIslandsServiceModel>>()))
                .AndAlso()
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<List<LatestIslandsServiceModel>>()
                .Passing(model => model.Count() == 3));
        }
    }
}
