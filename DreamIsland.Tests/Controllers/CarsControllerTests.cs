namespace DreamIsland.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using MyTested.AspNetCore.Mvc;
    using DreamIsland.Controllers;
    using DreamIsland.Services.Car.Models;
    using DreamIsland.Data.Models.Vehicles;
    using static Data.Cars;
    using DreamIsland.Tests.Mock.Car;

    public class CarsControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidIdOrInformation()
        {
            MyController<CarsController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }
    }
}
