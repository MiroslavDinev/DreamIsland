namespace DreamIsland.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    using DreamIsland.Data.Models.Collectibles;
    using DreamIsland.Data.Enums;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Tests.Mock;

    public class CollectiblesServiceTests
    {
        [Fact]
        public async Task AddCollectibleSuccessfulyAddsCollectiblesInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                ICollectibleService collectibleService = new CollectibleService(db, MapperMock.Instance);
                var collectibleId = await collectibleService.AddAsync(collectible.Name,collectible.Description, collectible.ImageUrl, 
                    collectible.RarityLevel, collectible.PartnerId);

                Assert.Equal(1, db.Collectibles.Count());
                Assert.Equal(1, collectibleId);
                Assert.Equal(collectible.Name, db.Collectibles.FirstOrDefault().Name);
            }
        }

        [Fact]
        public async Task EditCollectibleSuccessfulyEditsCollectibleInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                ICollectibleService collectibleService = new CollectibleService(db, MapperMock.Instance);
                var collectibleId = await collectibleService.AddAsync(collectible.Name, collectible.Description, collectible.ImageUrl,
                    collectible.RarityLevel, collectible.PartnerId);

                var edited = await collectibleService.EditAsync(collectibleId,"TestName","TestDescription", null,RarityLevel.Unique, true);

                Assert.Equal(1, db.Collectibles.Count());
                Assert.True(edited);
                Assert.Equal("TestName", db.Collectibles.FirstOrDefault().Name);
                Assert.Equal("TestDescription", db.Collectibles.FirstOrDefault().Description);
                Assert.Equal(RarityLevel.Unique.ToString(), db.Collectibles.FirstOrDefault().RarityLevel.ToString());
            }
        }

        [Fact]
        public async Task EditReturnsFalseIfCollectibleIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                ICollectibleService collectibleService = new CollectibleService(db, MapperMock.Instance);
                var collectibleId = await collectibleService.AddAsync(collectible.Name, collectible.Description, collectible.ImageUrl,
                    collectible.RarityLevel, collectible.PartnerId);

                var edited = await collectibleService.EditAsync(2, "TestName", "TestDescription", null, RarityLevel.Unique, true);

                Assert.False(edited);
            }
        }

        [Fact]
        public async Task DeleteCollectibleSuccessfulyDeletesCollectibleInDatabase()
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

            using (var db = DatabaseMock.Instance)
            {
                ICollectibleService collectibleService = new CollectibleService(db, MapperMock.Instance);
                var collectibleId = await collectibleService.AddAsync(collectible.Name, collectible.Description, collectible.ImageUrl,
                    collectible.RarityLevel, collectible.PartnerId);

                Assert.Equal(1, db.Collectibles.Count());
                Assert.Equal(1, collectibleId);
                Assert.Equal(collectible.Name, db.Collectibles.FirstOrDefault().Name);

                var isDeleted = collectibleService.Delete(1);

                Assert.True(isDeleted);
                Assert.True(db.Collectibles.FirstOrDefault().IsDeleted);
                Assert.False(db.Collectibles.FirstOrDefault().IsPublic);
            }
        }

        [Fact]
        public async Task DeleteCollectibleReturnFalseIfCollectibleIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                ICollectibleService collectibleService = new CollectibleService(db, MapperMock.Instance);
                var collectibleId = await collectibleService.AddAsync(collectible.Name, collectible.Description, collectible.ImageUrl,
                    collectible.RarityLevel, collectible.PartnerId);

                Assert.Equal(1, db.Collectibles.Count());
                Assert.Equal(1, collectibleId);
                Assert.Equal(collectible.Name, db.Collectibles.FirstOrDefault().Name);

                var isDeleted = collectibleService.Delete(2);

                Assert.False(isDeleted);
            }
        }

        [Fact]
        public void DeleteCollectibleReturnFalseIfCollectibleIsDeleted()
        {
            var collectible = new Collectible
            {
                Id = 1,
                Name = "Sword",
                RarityLevel = RarityLevel.Rare,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.Delete(collectible.Id);

            Assert.False(result);
        }

        [Fact]
        public void AllReturnsAllCollectiblesThatArePublic()
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
                IsPublic = true,
                PartnerId = 1
            };

            var otherCollectible = new Collectible
            {
                Id = 2,
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
            data.Collectibles.Add(otherCollectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.All();

            Assert.NotNull(result);
            Assert.Single(result.Collectibles);
        }

        [Fact]
        public void AllReturnsAllCollectiblesThatAreSpecificRarityLevel()
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
                IsPublic = true,
                PartnerId = 1
            };

            var otherCollectible = new Collectible
            {
                Id = 2,
                Name = "Sword",
                RarityLevel = RarityLevel.Unique,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = false,
                IsBooked = false,
                IsPublic = true,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.Collectibles.Add(otherCollectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.All(rarityLevel:"Rare");

            Assert.NotNull(result);
            Assert.Single(result.Collectibles);
            Assert.Equal("Rare", result.Collectibles.FirstOrDefault().RarityLevel);
        }

        [Fact]
        public void AllAdminReturnsAllCollectiblesThatAreNotDeleted()
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

            var otherCollectible = new Collectible
            {
                Id = 2,
                Name = "Sword",
                RarityLevel = RarityLevel.Rare,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.Collectibles.Add(otherCollectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.AllAdmin();

            Assert.NotNull(result);
            Assert.Single(result.Collectibles);
        }

        [Fact]
        public void ChangeStatusOfCollectibleWorksAsExpectedWithValidData()
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

            var result = collectibleService.ChangeStatus(collectible.Id);

            Assert.True(result);
        }

        [Fact]
        public void ChangeStatusOfCollectibleReturnFalseIfCollectibleIsDeleted()
        {
            var collectible = new Collectible
            {
                Id = 1,
                Name = "Sword",
                RarityLevel = RarityLevel.Rare,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectiblesService = new CollectibleService(data, MapperMock.Instance);

            var result = collectiblesService.ChangeStatus(collectible.Id);

            Assert.False(result);
        }

        [Fact]
        public void IsByPartnerReturnsTrueIfCollectibleIsAddedByThisPartner()
        {
            var collectible = new Collectible
            {
                Id = 1,
                Name = "Sword",
                RarityLevel = RarityLevel.Rare,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.IsByPartner(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void IsByPartnerReturnsFalseIfCollectibleIsNotAddedByThisPartner()
        {
            var collectible = new Collectible
            {
                Id = 1,
                Name = "Sword",
                RarityLevel = RarityLevel.Rare,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1
            };

            using var data = DatabaseMock.Instance;
            data.Collectibles.Add(collectible);
            data.SaveChanges();

            var collectibleService = new CollectibleService(data, MapperMock.Instance);

            var result = collectibleService.IsByPartner(1, 2);

            Assert.False(result);
        }
    }
}
