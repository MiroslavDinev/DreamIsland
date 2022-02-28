namespace DreamIsland.Tests.Pipeline
{
    using System.Linq;

    using Xunit;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Collectible;
    using DreamIsland.Data.Enums;
    using DreamIsland.Data.Models.Collectibles;

    using static WebConstants;

    public class AdminCollectiblesControllerTests
    {
        [Fact]
        public void GetAllShouldReturnViewWithAllCollectibles()
        {
            MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Collectibles/All")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CollectiblesController>(c => c.All(new AllAdminCollectiblesQueryModel()))
                .Which(controller => controller
                    .WithData(new Collectible
                    {
                        Id = 1,
                        Name = "Sword",
                        RarityLevel = RarityLevel.Rare,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1
                    })
                .ShouldReturn()
                .View(view => view
                    .WithModelOfType<AllAdminCollectiblesQueryModel>()));
        }

        [Fact]
        public void GetMineShouldChangeStatusCollectiblesAndRedirectToAll()
            => MyMvc
                .Pipeline()
                .ShouldMap(request => request
                    .WithLocation("/Admin/Collectibles/ChangeStatus/1")
                    .WithUser(new[] { AdministratorRoleName }))
                .To<CollectiblesController>(c => c.ChangeStatus(1))
                .Which(controller => controller
                    .WithData(new Collectible
                    {
                        Id = 1,
                        Name = "Sword",
                        RarityLevel = RarityLevel.Rare,
                        ImageUrl = null,
                        Description = "Test test test test test",
                        IsDeleted = false,
                        IsBooked = false,
                        IsPublic = false,
                        PartnerId = 1
                    }))
                .ShouldHave()
                .Data(data => data
                    .WithSet<Collectible>(set =>
                    {
                        var collectible = set.FirstOrDefault(c => c.IsPublic);
                    }))
                .AndAlso()
                .ShouldReturn()
                .Redirect();
    }
}
