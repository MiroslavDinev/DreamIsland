namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Services.Car;
    using DreamIsland.Tests.Mock;
    using Areas.Admin.Models.Car;

    public class AdminCarsControllerTests
    {           
        [Fact]
        public void AllReturnsEvenNotPublicCars()
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
                IsPublic = false,
                Year = 2020,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Cars.Add(car);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var adminCarsController = new CarsController(carService);

            var result = adminCarsController.All(new AllAdminCarsQueryModel() { CurrentPage = 1});

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void ChangeStatusReturnsNotFoundIfNoSuchCar()
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
                IsPublic = false,
                Year = 2020,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Cars.Add(car);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var adminCarsController = new CarsController(carService);

            var result = adminCarsController.ChangeStatus(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ChangeStatusRedirectsWithCorrectCarId()
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
                IsPublic = false,
                Year = 2020,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Cars.Add(car);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var adminCarsController = new CarsController(carService);

            var result = (RedirectToActionResult) adminCarsController.ChangeStatus(car.Id);

            Assert.Equal("All", result.ActionName);
        }
    }
}
