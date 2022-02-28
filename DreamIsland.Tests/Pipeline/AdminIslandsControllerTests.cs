namespace DreamIsland.Tests.Pipeline
{
    using System.Linq;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Island;
    using DreamIsland.Data.Models.Islands;

    using static WebConstants;

    public class AdminIslandsControllerTests
    {
        [Fact]
        public void GetAllShouldReturnViewWithAllIslands()
        {
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Islands/All")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<IslandsController>(c => c.All(new AllAdminIslandsQueryModel()))
                .Which(controller => controller
                    .WithData(new Island
                    {
                        Id = 1,
                        Name = "Halkidiki",
                        IslandRegion = new IslandRegion { Name = "Europe" },
                        IslandRegionId = 1,
                        Location = "Greece",
                        PopulationSize = new PopulationSize { Name = "Up to 10 persons" },
                        PopulationSizeId = 1,
                        Price = 1000000000,
                        SizeInSquareKm = 12,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1
                    })
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllAdminIslandsQueryModel>()));
        }

        [Fact]
        public void GetMineShouldChangeStatusIslandAndRedirectToAll()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Islands/ChangeStatus/1")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<IslandsController>(c => c.ChangeStatus(1))
                .Which(controller => controller
                    .WithData(new Island
                    {
                        Id = 1,
                        Name = "Halkidiki",
                        IslandRegion = new IslandRegion { Name = "Europe" },
                        IslandRegionId = 1,
                        Location = "Greece",
                        PopulationSize = new PopulationSize { Name = "Up to 10 persons" },
                        PopulationSizeId = 1,
                        Price = 1000000000,
                        SizeInSquareKm = 12,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1
                    }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Island>(set =>
                    {
                        var island = set.FirstOrDefault(i => i.IsPublic);
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect();
    }
}
