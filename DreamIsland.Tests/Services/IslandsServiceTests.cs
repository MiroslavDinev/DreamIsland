namespace DreamIsland.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Services.Island;
    using DreamIsland.Tests.Mock;

    public class IslandsServiceTests
    {
        [Fact]
        public async Task AddIslandSuccessfulyAddsIslandInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                IIslandService islandService = new IslandService(db, MapperMock.Instance);
                var islandId = await islandService.AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price, 
                    island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, island.PartnerId);

                Assert.Equal(1, db.Islands.Count());
                Assert.Equal(1, islandId);
                Assert.Equal(island.Name, db.Islands.FirstOrDefault().Name);
            }
        }

        [Fact]
        public async Task EditCollectibleSuccessfulyEditsCollectibleInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                IIslandService islandService = new IslandService(db, MapperMock.Instance);
                var islandId = await islandService.AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price,
                    island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, island.PartnerId);

                var edited = await islandService.EditAsync(islandId, "TestName", "TestLocation", "TestDescription", 
                    1, 1, null,1,1,true);

                Assert.Equal(1, db.Islands.Count());
                Assert.True(edited);
                Assert.Equal("TestName", db.Islands.FirstOrDefault().Name);
                Assert.Equal("TestDescription", db.Islands.FirstOrDefault().Description);
                Assert.Equal(1, db.Islands.FirstOrDefault().PopulationSizeId);
            }
        }

        [Fact]
        public async Task DeleteCollectibleSuccessfulyDeletesCollectibleInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                IIslandService islandService = new IslandService(db, MapperMock.Instance);
                var islandId = await islandService.AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price,
                    island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, island.PartnerId);

                Assert.Equal(1, db.Islands.Count());
                Assert.Equal(1, islandId);
                Assert.Equal(island.Name, db.Islands.FirstOrDefault().Name);

                var isDeleted = islandService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Islands.FirstOrDefault().IsDeleted);
                Assert.False(db.Islands.FirstOrDefault().IsPublic);
            }
        }

        [Fact]
        public void DeleteIslandReturnFalseIfIslandIsDeleted()
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
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.Delete(island.Id);

            Assert.False(result);
        }

        [Fact]
        public void AllReturnsAllIslandsThatArePublic()
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
                IsPublic = true,
                PartnerId = 1
            };

            var anotherIsland = new Island
            {
                Id = 2,
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
            data.Islands.Add(anotherIsland);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.All();

            Assert.NotNull(result);
            Assert.Single(result.Islands);
        }

        [Fact]
        public void AllAdminReturnsAllIslandsThatAreNotDeleted()
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

            var anotherIsland = new Island
            {
                Id = 2,
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
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.Islands.Add(anotherIsland);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.AllAdmin();

            Assert.NotNull(result);
            Assert.Single(result.Islands);
        }

        [Fact]
        public void ChangeStatusOfIslandWorksAsExpectedWithValidData()
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

            var result = islandService.ChangeStatus(island.Id);

            Assert.True(result);
        }

        [Fact]
        public void ChangeStatusOfIslandReturnFalseIfIslandIsDeleted()
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
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.ChangeStatus(island.Id);

            Assert.False(result);
        }
    }
}
