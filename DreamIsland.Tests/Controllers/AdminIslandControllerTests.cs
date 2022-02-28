namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Island;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Services.Island;
    using DreamIsland.Tests.Mock;

    public class AdminIslandControllerTests
    {
        [Fact]
        public void AllReturnsEvenNotPublicIslands()
        {
            var island = new Island
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
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var adminIslandsController = new IslandsController(islandService);

            var result = adminIslandsController.All(new AllAdminIslandsQueryModel() { CurrentPage = 1 });

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public void ChangeStatusReturnsNotFoundIfNoSuchIsland()
        {
            var island = new Island
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
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var adminIslandsController = new IslandsController(islandService);

            var result = adminIslandsController.ChangeStatus(2);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ChangeStatusRedirectsWithCorrectCarId()
        {
            var island = new Island
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
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var adminIslandsController = new IslandsController(islandService);

            var result = (RedirectToActionResult)adminIslandsController.ChangeStatus(island.Id);

            Assert.Equal("All", result.ActionName);
        }
    }
}
