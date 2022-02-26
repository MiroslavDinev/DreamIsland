namespace DreamIsland.Tests.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;
    using Microsoft.EntityFrameworkCore;

    using DreamIsland.Data;
    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Tests.Mock;

    public class CelebritiesControllerTests
    {
        [Fact]
        public async Task AddCelebritySuccessfulyAddsCelebritiesInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICelebrityService celebrityService = new CelebrityService(db, MapperMock.Instance);
                var celebrityId = await celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, 
                    celebrity.ImageUrl, celebrity.Age, celebrity.PartnerId);

                Assert.Equal(1, db.Celebrities.Count());
                Assert.Equal(1, celebrityId);
                Assert.Equal(celebrity.Name, db.Celebrities.FirstOrDefault().Name);
            }
        }

        [Fact]
        public async Task EditCelebritySuccessfulyEditsCelebrityInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICelebrityService celebrityService = new CelebrityService(db, MapperMock.Instance);
                var celebrityId = await celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description,
                    celebrity.ImageUrl, celebrity.Age, celebrity.PartnerId);

                var edited = await celebrityService.EditAsync(celebrityId, "TestEdited", "TestovEdited", "EditedTestDescription",
                    null, 20, true);

                Assert.Equal(1, db.Celebrities.Count());
                Assert.True(edited);
                Assert.Equal("TestEdited", db.Celebrities.FirstOrDefault().Name);
                Assert.Equal("TestovEdited", db.Celebrities.FirstOrDefault().Occupation);
                Assert.Equal("EditedTestDescription", db.Celebrities.FirstOrDefault().Description);
                Assert.Equal(20, db.Celebrities.FirstOrDefault().Age);
            }
        }

        [Fact]
        public async Task DeleteCelebritySuccessfulyDeletesCelebrityInDatabase()
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

            var options = new DbContextOptionsBuilder<DreamIslandDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using (var db = new DreamIslandDbContext(options))
            {
                ICelebrityService celebrityService = new CelebrityService(db, MapperMock.Instance);
                var celebrityId = await celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description,
                    celebrity.ImageUrl, celebrity.Age, celebrity.PartnerId);

                Assert.Equal(1, db.Celebrities.Count());
                Assert.Equal(1, celebrityId);
                Assert.Equal(celebrity.Name, db.Celebrities.FirstOrDefault().Name);

                var isDeleted = celebrityService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Celebrities.FirstOrDefault().IsDeleted);
                Assert.False(db.Celebrities.FirstOrDefault().IsPublic);
            }
        }
    }
}
