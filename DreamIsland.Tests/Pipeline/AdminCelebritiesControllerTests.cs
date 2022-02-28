namespace DreamIsland.Tests.Pipeline
{
    using System.Linq;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Celebrity;
    using DreamIsland.Data.Models.Celebrities;

    using static WebConstants;

    public class AdminCelebritiesControllerTests
    {
        [Fact]
        public void GetAllShouldReturnViewWithAllCelebrities()
        {
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Celebrities/All")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CelebritiesController>(c => c.All(new AllAdminCelebritiesQueryModel()))
                .Which(controller => controller
                    .WithData(new Celebrity
                    {
                        Id = 1,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1,
                        Name = "TestName",
                        Occupation = "TestOccupation",
                        Age = 30
                    })
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllAdminCelebritiesQueryModel>()));
        }

        [Fact]
        public void GetMineShouldChangeStatusCelebrityAndRedirectToAll()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Celebrities/ChangeStatus/1")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CelebritiesController>(c => c.ChangeStatus(1))
                .Which(controller => controller
                    .WithData(new Celebrity
                    {
                        Id = 1,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1,
                        Name = "TestName",
                        Occupation = "TestOccupation",
                        Age = 30
                    }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Celebrity>(set =>
                    {
                        var celebrity = set.FirstOrDefault(c => c.IsPublic);
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect();
    }
}
