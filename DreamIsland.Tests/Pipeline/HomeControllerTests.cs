namespace DreamIsland.Tests.Pipeline
{
    using System.Linq;
    using System.Collections.Generic;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Services.Island.Models;

    using static Data.Islands;

    public class HomeControllerTests
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
        {
            MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
                .Which(controller => controller
                .WithData(TenPublicIslands())
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<List<LatestIslandsServiceModel>>()
                .Passing(i => i.Count() == 3)));
        }

        [Fact]
        public void PrivacyShouldReturnView()
        {
            MyMvc
                .Pipeline()
                .ShouldMap("/Home/Privacy")
                .To<HomeController>(c => c.Privacy())
                .Which()
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void ChatShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Home/Chat")
                .WithUser())
                .To<HomeController>(c => c.Chat())
                .Which()
                .ShouldHave()
                .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
                .AndAlso()
                .ShouldReturn()
                .View();
        }
    }
}
