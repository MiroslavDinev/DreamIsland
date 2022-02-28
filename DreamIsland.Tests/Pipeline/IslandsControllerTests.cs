namespace DreamIsland.Tests.Pipeline
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Islands;

    public class IslandsControllerTests
    {
        [Fact]
        public void AllShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Islands/All"))
                .To<IslandsController>(c => c.All(new AllIslandsQueryModel()))
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
                .WithPath("/Islands/My")
                .WithUser())
                .To<IslandsController>(c => c.My())
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
