namespace DreamIsland.Tests.Pipeline
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Collectibles;

    public class CollectiblesControllerTests
    {
        [Fact]
        public void AllShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Collectibles/All"))
                .To<CollectiblesController>(c => c.All(new AllCollectiblesQueryModel()))
                .Which()
                .ShouldReturn()
                .View();
        }

        [Fact]
        public void MyShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Collectibles/My")
                .WithUser())
                .To<CollectiblesController>(c => c.My())
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
