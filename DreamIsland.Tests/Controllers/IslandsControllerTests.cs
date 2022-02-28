namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Models.Islands;
    using DreamIsland.Services.Island;
    using DreamIsland.Services.Partner;
    using DreamIsland.Tests.Mock;

    public class IslandsControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidIslandId()
        {
            MyController<IslandsController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void AllReturnsViewWithIslandsFromDatabase()
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
            var partnerService = new PartnerService(data);

            var islandsController = new IslandsController(islandService, partnerService,
                MapperMock.Instance, new Mock<IWebHostEnvironment>().Object);

            var result = islandsController.All(new AllIslandsQueryModel());

            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
