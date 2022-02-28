namespace DreamIsland.Tests.Pipeline
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Models.Celebrities;

    public class CelebritiesControllerTests
    {
        [Fact]
        public void AllShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Celebrities/All"))
                .To<CelebritiesController>(c => c.All(new AllCelebritiesQueryModel()))
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
                .WithPath("/Celebrities/My")
                .WithUser())
                .To<CelebritiesController>(c => c.My())
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
