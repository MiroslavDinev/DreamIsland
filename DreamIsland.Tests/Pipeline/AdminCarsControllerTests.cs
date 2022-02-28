namespace DreamIsland.Tests.Pipeline
{
    using System.Linq;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Data.Models.Vehicles;
    using Areas.Admin.Models.Car;

    using static WebConstants;

    public class AdminCarsControllerTests
    {
        [Fact]
        public void GetAllShouldReturnViewWithAllCars()
        {
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Cars/All")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CarsController>(c => c.All(new AllAdminCarsQueryModel()))
                .Which(controller => controller
                    .WithData(new Car
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
                    })
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllAdminCarsQueryModel>()));
        }

        [Fact]
        public void GetMineShouldChangeStatusCarAndRedirectToAll()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Cars/ChangeStatus/1")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CarsController>(c => c.ChangeStatus(1))
                .Which(controller => controller
                    .WithData(new Car
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
                    }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Car>(set =>
                    {
                        var car = set.FirstOrDefault(c => c.IsPublic);
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect();
    }
}
