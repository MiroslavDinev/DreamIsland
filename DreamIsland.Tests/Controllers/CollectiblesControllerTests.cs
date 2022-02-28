namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Data.Enums;
    using DreamIsland.Data.Models.Collectibles;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Services.Partner;
    using DreamIsland.Tests.Mock;

    public class CollectiblesControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidCollectibleId()
        {
            MyController<CollectiblesController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void AllReturnsViewWithCollectibleFromDatabase()
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
            var partnerService = new PartnerService(data);

            var collectiblesController = new CollectiblesController(collectibleService, partnerService,
                MapperMock.Instance, new Mock<IWebHostEnvironment>().Object);

            var result = collectiblesController.All(new AllCollectiblesQueryModel());

            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
