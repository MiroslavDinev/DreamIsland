namespace DreamIsland.Tests.Controllers
{
    using System.Threading.Tasks;

    using Xunit;
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Areas.Admin.Controllers;
    using DreamIsland.Areas.Admin.Models.Celebrity;
    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Tests.Mock;

    public class AdminCelebritiesControllerTests
    {
        [Fact]
        public void AllReturnsEvenNotPublicCelebrities()
        {
            var celebrity = new Celebrity
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
            };

            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(celebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);

            var adminCelebritiesController = new CelebritiesController(celebrityService);

            var result = adminCelebritiesController.All(new AllAdminCelebritiesQueryModel() { CurrentPage = 1 });

            Assert.IsAssignableFrom<ViewResult>(result);
        }

        [Fact]
        public async Task ChangeStatusRedirectsWithCorrectCelebrityId()
        {
            var celebrity = new Celebrity
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
            };

            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(celebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);

            var adminCelebritiesController = new CelebritiesController(celebrityService);

            var result = (RedirectToActionResult) await adminCelebritiesController.ChangeStatus(celebrity.Id);

            Assert.Equal("All", result.ActionName);
        }
    }
}
