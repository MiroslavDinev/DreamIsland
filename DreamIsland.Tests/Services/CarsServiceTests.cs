namespace DreamIsland.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    using DreamIsland.Data.Models.Vehicles;
    using DreamIsland.Services.Car;
    using DreamIsland.Tests.Mock;

    public class CarsServiceTests
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

            using (var db = DatabaseMock.Instance)
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

            using (var db = DatabaseMock.Instance)
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
        public async Task EditReturnsFalseIfCarIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                ICarService carService = new CarService(db, MapperMock.Instance);
                var carId = await carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.HasRemoteStart,
                    car.HasRemoteControlParking, car.HasSeatMassager, car.PartnerId);

                var edited = await carService.EditAsync(2, "TestEdited", "TestovEdited", "EditedTestDescription",
                    null, 2022, true, true, true, false);

                Assert.False(edited);               
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

            using (var db = DatabaseMock.Instance)
            {
                ICarService carService = new CarService(db, MapperMock.Instance);
                var carId = await carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.HasRemoteStart,
                    car.HasRemoteControlParking, car.HasSeatMassager, car.PartnerId);

                Assert.Equal(1, db.Cars.Count());
                Assert.Equal(1, carId);
                Assert.Equal(car.Model, db.Cars.FirstOrDefault().Model);

                var isDeleted = await carService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Cars.FirstOrDefault().IsDeleted);
                Assert.False(db.Cars.FirstOrDefault().IsPublic);
            }
        }

        [Fact]
        public async Task DeleteCarReturnFalseIfCarIsDeleted()
        {
            var car = new Car
            {
                Id = 1,
                Brand = "Test",
                Model = "Testov",
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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

            var result =await carService.Delete(car.Id);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteCarReturnFalseIfCarIsNull()
        {
            var car = new Car
            {
                Id = 1,
                Brand = "Test",
                Model = "Testov",
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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

            var result =await carService.Delete(2);

            Assert.False(result);
        }

        [Fact]
        public void AllReturnsAllCarsThatArePublic()
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

            var otherCar = new Car
            {
                Id = 2,
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
            data.Cars.Add(otherCar);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var result = carService.All();

            Assert.NotNull(result);
            Assert.Single(result.Cars);
        }

        [Fact]
        public void AllReturnsAllCarsThatAreSpecificBrand()
        {
            var car = new Car
            {
                Id = 1,
                Brand = "Audi",
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

            var otherCar = new Car
            {
                Id = 2,
                Brand = "Mercedes",
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
            data.Cars.Add(otherCar);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var result = carService.All(brand:"Audi");

            Assert.NotNull(result);
            Assert.Single(result.Cars);
            Assert.Equal("Audi", result.Cars.FirstOrDefault().Brand);
        }

        [Fact]
        public void AllAdminReturnsAllCarsThatAreNotDeleted()
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

            var otherCar = new Car
            {
                Id = 2,
                Brand = "Test",
                Model = "Testov",
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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
            data.Cars.Add(otherCar);
            data.SaveChanges();

            var carService = new CarService(data, MapperMock.Instance);

            var result = carService.AllAdmin();

            Assert.NotNull(result);
            Assert.Single(result.Cars);
        }

        [Fact]
        public async Task ChangeStatusOfCarWorksAsExpectedWithValidData()
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

            var result =await carService.ChangeStatus(car.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task ChangeStatusOfCarReturnFalseIfCarIsDeleted()
        {
            var car = new Car
            {
                Id = 1,
                Brand = "Test",
                Model = "Testov",
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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

            var result =await carService.ChangeStatus(car.Id);

            Assert.False(result);
        }

        [Fact]
        public void IsByPartnerReturnsTrueIfCarIsAddedByThisPartner()
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

            var result = carService.IsByPartner(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void IsByPartnerReturnsFalseIfCarIsNotAddedByThisPartner()
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

            var result = carService.IsByPartner(1, 2);

            Assert.False(result);
        }

    }
}
