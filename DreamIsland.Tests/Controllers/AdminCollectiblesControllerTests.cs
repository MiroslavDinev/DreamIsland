namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Collectible;
    using DreamIsland.Data.Enums;
    using DreamIsland.Data.Models.Collectibles;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Tests.Mock;

    public class AdminCollectiblesControllerTests
    {
        [Fact]
        public void AllReturnsEvenNotPublicCollectibles()
        {
            var collectible = new Collectible
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
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var adminCollectiblesController = new CollectiblesController(collectibleService);

            var result = adminCollectiblesController.All(new AllAdminCollectiblesQueryModel() { CurrentPage = 1 });

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void ChangeStatusReturnsNotFoundIfNoSuchCollectible()
        {
            var collectible = new Collectible
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
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var adminCollectiblesController = new CollectiblesController(collectibleService);

            var result = adminCollectiblesController.ChangeStatus(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ChangeStatusRedirectsWithCorrectCollectibleId()
        {
            var collectible = new Collectible
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
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var adminCollectiblesController = new CollectiblesController(collectibleService);

            var result = (RedirectToActionResult)adminCollectiblesController.ChangeStatus(collectible.Id);

            Assert.Equal("All", result.ActionName);
        }
    }
}
