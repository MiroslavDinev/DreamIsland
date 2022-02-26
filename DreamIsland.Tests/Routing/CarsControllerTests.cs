namespace DreamIsland.Tests.Routing
{
    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using Models.Cars;

    public class CarsControllerTests
    {
        [Fact]
        public void AllRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/All")
                .To<CarsController>(c => c.All(new AllCarsQueryModel()));
        }

        [Fact]
        public void MyRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/My")
                .To<CarsController>(c => c.My());
        }

        [Fact]
        public void GetAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/Add")
                .To<CarsController>(c => c.Add());
        }

        [Fact]
        public void PostAddRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Cars/Add")
                .WithMethod(HttpMethod.Post))
                .To<CarsController>(c => c.Add(With.Any<CarAddFormModel>()));
                
        }

        [Fact]
        public void GetEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/Edit/1")
                .To<CarsController>(c => c.Edit(1));
        }

        [Fact]
        public void PostEditRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap(request => request
                .WithPath("/Cars/Edit")
                .WithMethod(HttpMethod.Post))
                .To<CarsController>(c => c.Edit(With.Any<CarEditFormModel>()));

        }

        [Fact]
        public void DeleteRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/Delete/1")
                .To<CarsController>(c => c.Delete(1));
        }

        [Fact]
        public void DetailsRouteShouldBeMapped()
        {
            MyRouting
                .Configuration()
                .ShouldMap("/Cars/Details/1/Audi-RS6")
                .To<CarsController>(c => c.Details(1, "Audi-RS6"));
        }
    }
}
