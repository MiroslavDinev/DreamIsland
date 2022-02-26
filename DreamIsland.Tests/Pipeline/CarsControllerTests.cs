namespace DreamIsland.Tests.Pipeline
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using Models.Cars;

    public class CarsControllerTests
    {
        [Fact]
        public void AllShouldReturnView()
        {
            MyPipeline
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Cars/All"))
                .To<CarsController>(c => c.All(new AllCarsQueryModel()))
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
                .WithPath("/Cars/My")
                .WithUser())
                .To<CarsController>(c => c.My())
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
