namespace DreamIsland.Tests.Controllers
{
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using MyTested.AspNetCore.Mvc;

    using DreamIsland.Controllers;
    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Partner;
    using DreamIsland.Tests.Mock;

    public class CelebritiesControllerTests
    {
        [Fact]
        public void DetailsShouldReturnNotFoundWhenInvalidCelebrityId()
        {
            MyController<CelebritiesController>
                .Calling(c => c.Details(int.MaxValue, string.Empty))
                .ShouldReturn()
                .NotFound();
        }

        [Fact]
        public void AllReturnsViewWithCelebritiesFromDatabase()
        {
            var celebrity = new Celebrity
            {
                Id = 1,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = false,
                IsBooked = false,
                IsPublic = true,
                PartnerId = 1,
                Name = "TestName",
                Occupation = "TestOccupation",
                Age = 30
            };

            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(celebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);
            var partnerService = new PartnerService(data);

            var celebritiesController = new CelebritiesController(celebrityService, partnerService,
                MapperMock.Instance, new Mock<IWebHostEnvironment>().Object);

            var result = celebritiesController.All(new AllCelebritiesQueryModel());

            Assert.IsAssignableFrom<ViewResult>(result);
        }
    }
}
