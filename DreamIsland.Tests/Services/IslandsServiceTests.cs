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
        public async Task EditReturnsFalseIfIslandIsNull()
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

                var edited = await islandService.EditAsync(2, "TestName", "TestLocation", "TestDescription",
                    1, 1, null, 1, 1, true);

                Assert.False(edited);
            }
        }

        [Fact]
        public async Task DeleteIslandSuccessfulyDeletesIslandInDatabase()
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

                var isDeleted =await islandService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Islands.FirstOrDefault().IsDeleted);
                Assert.False(db.Islands.FirstOrDefault().IsPublic);
            }
        }

        [Fact]
        public async Task DeleteIslandReturnFalseIfIslandIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                IIslandService islandService = new IslandService(db, MapperMock.Instance);
                var islandId = await islandService.AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price,
                    island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, island.PartnerId);

                Assert.Equal(1, db.Islands.Count());
                Assert.Equal(1, islandId);
                Assert.Equal(island.Name, db.Islands.FirstOrDefault().Name);

                var isDeleted =await islandService.Delete(2);

                Assert.False(isDeleted);
            }
        }

        [Fact]
        public async Task DeleteIslandReturnFalseIfIslandIsDeleted()
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

            var result =await islandService.Delete(island.Id);

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
        public void AllReturnsAllIslandsThatAreSpecificName()
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
                Name = "Test",
                IslandRegion = new IslandRegion { Name = "Asia" },
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

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.Islands.Add(anotherIsland);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.All(searchTerm:"Test");

            Assert.NotNull(result);
            Assert.Single(result.Islands);
            Assert.Equal("Test", result.Islands.FirstOrDefault().Name);
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
        public async Task ChangeStatusOfIslandWorksAsExpectedWithValidData()
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

            var result =await islandService.ChangeStatus(island.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task ChangeStatusOfIslandReturnFalseIfIslandIsDeleted()
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

            var result =await islandService.ChangeStatus(island.Id);

            Assert.False(result);
        }

        [Fact]
        public void GetPopulationSizesReturnsPopulationSizesIsInDatabase()
        {
            var populationSize = new PopulationSize { Id = 1, Name = "Up to 10 persons" };
            var populationSize2 = new PopulationSize { Id = 2, Name = "Up to 100 persons" };
            var populationSize3 = new PopulationSize { Id = 3, Name = "Up to 1000 persons" };
            var populationSize4 = new PopulationSize { Id = 4, Name = "Up to 10000 persons" };

            using var data = DatabaseMock.Instance;
            data.PopulationSizes.Add(populationSize);
            data.PopulationSizes.Add(populationSize2);
            data.PopulationSizes.Add(populationSize3);
            data.PopulationSizes.Add(populationSize4);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.GetPopulationSizes();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void PopulationSizeExistsReturnsTrueIfSizeIsInDatabase()
        {
            var populationSize = new PopulationSize { Id = 1, Name = "Up to 10 persons" };

            using var data = DatabaseMock.Instance;
            data.PopulationSizes.Add(populationSize);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.PopulationSizeExists(populationSize.Id);

            Assert.True(result);
        }

        [Fact]
        public void PopulationSizeExistsReturnsFalseIfSizeIsNotInDatabase()
        {
            var populationSize = new PopulationSize { Id = 1, Name = "Up to 10 persons" };

            using var data = DatabaseMock.Instance;
            data.PopulationSizes.Add(populationSize);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.PopulationSizeExists(2);

            Assert.False(result);
        }

        [Fact]
        public void GetRegionsReturnsTheRegionsIsInDatabase()
        {
            var region = new IslandRegion { Id = 1, Name = "Europe" };
            var region1 = new IslandRegion { Id = 2, Name = "Africa" };
            var region2 = new IslandRegion { Id = 3, Name = "Asia" };
            var region3 = new IslandRegion { Id = 4, Name = "Australia" };

            using var data = DatabaseMock.Instance;
            data.IslandRegions.Add(region);
            data.IslandRegions.Add(region1);
            data.IslandRegions.Add(region2);
            data.IslandRegions.Add(region3);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.GetRegions();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public void RegionExistsReturnsTrueIfRegionIsInDatabase()
        {
            var region = new IslandRegion {Id =1, Name = "Europe" };

            using var data = DatabaseMock.Instance;
            data.IslandRegions.Add(region);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.RegionExists(region.Id);

            Assert.True(result);
        }

        [Fact]
        public void RegionExistsReturnsFalseIfRegionIsNotInDatabase()
        {
            var region = new IslandRegion { Id = 1, Name = "Europe" };

            using var data = DatabaseMock.Instance;
            data.IslandRegions.Add(region);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.RegionExists(2);

            Assert.False(result);
        }

        [Fact]
        public void LatestIslandReturnsLastThreeIslandsFromTheDatabase()
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

            var island2 = new Island
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
                IsPublic = true,
                PartnerId = 1
            };

            var island3 = new Island
            {
                Id = 3,
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

            var island4 = new Island
            {
                Id = 4,
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

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.Islands.Add(island2);
            data.Islands.Add(island3);
            data.Islands.Add(island4);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.LatestIslands();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public void IsByPartnerReturnsTrueIfIslandIsAddedByThisPartner()
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

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.IsByPartner(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void IsByPartnerReturnsFalseIfIslandIsNotAddedByThisPartner()
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

            using var data = DatabaseMock.Instance;
            data.Islands.Add(island);
            data.SaveChanges();

            var islandService = new IslandService(data, MapperMock.Instance);

            var result = islandService.IsByPartner(1, 2);

            Assert.False(result);
        }
    }
}
