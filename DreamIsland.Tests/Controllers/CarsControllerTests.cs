namespace DreamIsland.Tests.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Microsoft.EntityFrameworkCore;

    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Data;
    using DreamIsland.Services.Car;
    using DreamIsland.Tests.Mock;

    public class CarsControllerTests
    {
        [Fact]
        public async Task AddCarSuccessfulyAddsCarsInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICarService carService = new CarService(db, MapperMock.Instance);
                var carId = await carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.HasRemoteStart, 
                    car.HasRemoteControlParking, car.HasSeatMassager, car.PartnerId);

                Assert.Equal(1, db.Cars.Count());
                Assert.Equal(1, carId);
                Assert.Equal(car.Model, db.Cars.FirstOrDefault().Model);
            }
        }

        [Fact]
        public async Task EditCarSuccessfulyEditsCarsInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICarService carService = new CarService(db, MapperMock.Instance);
                var carId = await carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.HasRemoteStart,
                    car.HasRemoteControlParking, car.HasSeatMassager, car.PartnerId);

                var edited = await carService.EditAsync(carId, "TestEdited", "TestovEdited", "EditedTestDescription", 
                    null, 2022, true, true, true, false);

                Assert.Equal(1, db.Cars.Count());
                Assert.True(edited);
                Assert.Equal("TestEdited", db.Cars.FirstOrDefault().Brand);
                Assert.Equal("TestovEdited", db.Cars.FirstOrDefault().Model);
                Assert.Equal("EditedTestDescription", db.Cars.FirstOrDefault().Description);
                Assert.Equal(2022, db.Cars.FirstOrDefault().Year);
            }
        }

        [Fact]
        public async Task DeleteCarSuccessfulyDeletesCarsInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICarService carService = new CarService(db, MapperMock.Instance);
                var carId = await carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.HasRemoteStart,
                    car.HasRemoteControlParking, car.HasSeatMassager, car.PartnerId);

                Assert.Equal(1, db.Cars.Count());
                Assert.Equal(1, carId);
                Assert.Equal(car.Model, db.Cars.FirstOrDefault().Model);

                var isDeleted = carService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Cars.FirstOrDefault().IsDeleted);
                Assert.False(db.Cars.FirstOrDefault().IsPublic);
            }
        }
    }
}
