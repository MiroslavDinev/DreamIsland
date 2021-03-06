namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Tests.Mock;
    using DreamIsland.Services.Car;
    using DreamIsland.Services.Partner;
    using Models.Cars;

    public class CarsControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidCarId()
        {
            MyController<CarsController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void AllReturnsViewWithCarsFromDatabase()
        {
            var car = new Car
            {
                Id = 1,
                Brand = "Test",
                Model = "Testov",
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = false,
                IsBooked = false,
                HasRemoteControlParking = false,
                HasRemoteStart = false,
                HasSeatMassager = false,
                IsPublic = true,
                Year = 2020,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Cars.Add(car);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);
            var partnerService = new PartnerService(data);

            var carsController = new CarsController(carService, partnerService, 
                MapperMock.Instance, new Mock<IWebHostEnvironment>().Object);

            var result = carsController.All(new AllCarsQueryModel());

            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
