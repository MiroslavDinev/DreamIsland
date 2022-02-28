namespace DreamIsland.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Tests.Mock;

    public class CelebritiesServiceTests
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

            using (var db = DatabaseMock.Instance)
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

            using (var db = DatabaseMock.Instance)
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
        public async Task EditReturnsFalseIfCelebrityIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                ICelebrityService celebrityService = new CelebrityService(db, MapperMock.Instance);
                var celebrityId = await celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description,
                    celebrity.ImageUrl, celebrity.Age, celebrity.PartnerId);

                var edited = await celebrityService.EditAsync(2, "TestEdited", "TestovEdited", "EditedTestDescription",
                    null, 20, true);

                Assert.False(edited);               
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

            using (var db = DatabaseMock.Instance)
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

        [Fact]
        public async Task DeleteCelebrityReturnFalseIfCelebrityIsNull()
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

            using (var db = DatabaseMock.Instance)
            {
                ICelebrityService celebrityService = new CelebrityService(db, MapperMock.Instance);
                var celebrityId = await celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description,
                    celebrity.ImageUrl, celebrity.Age, celebrity.PartnerId);

                Assert.Equal(1, db.Celebrities.Count());
                Assert.Equal(1, celebrityId);
                Assert.Equal(celebrity.Name, db.Celebrities.FirstOrDefault().Name);

                var isDeleted = celebrityService.Delete(2);

                Assert.False(isDeleted);
            }
        }

        [Fact]
        public void DeleteCelebrityReturnFalseIfCelebrityIsDeleted()
        {
            var celebrity = new Celebrity
            {
                Id = 1,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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

            var celebrityServie = new CelebrityService(data, MapperMock.Instance);

            var result = celebrityServie.Delete(celebrity.Id);

            Assert.False(result);
        }

        [Fact]
        public void AllReturnsAllCelebritiesThatArePublic()
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

            var otherCelebrity = new Celebrity
            {
                Id = 2,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1,
                Name = "TestName",
                Occupation = "TestOccupation",
                Age = 30
            };

            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(celebrity);
            data.Celebrities.Add(otherCelebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);

            var result = celebrityService.All();

            Assert.NotNull(result);
            Assert.Single(result.Celebrities);
        }

        [Fact]
        public void AllReturnsAllCelebritiesThatAreSpecificOccupation()
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
                Occupation = "Actor",
                Age = 30
            };

            var otherCelebrity = new Celebrity
            {
                Id = 2,
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
            data.Celebrities.Add(otherCelebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);

            var result = celebrityService.All(occupation:"Actor");

            Assert.NotNull(result);
            Assert.Single(result.Celebrities);
            Assert.Equal("Actor", result.Celebrities.FirstOrDefault().Occupation);
        }

        [Fact]
        public void AllAdminReturnsAllCelebritiesThatAreNotDeleted()
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

            var otherCelebrity = new Celebrity
            {
                Id = 2,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
                IsBooked = false,
                IsPublic = false,
                PartnerId = 1,
                Name = "TestName",
                Occupation = "TestOccupation",
                Age = 30
            };

            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(celebrity);
            data.Celebrities.Add(otherCelebrity);
            data.SaveChanges();

            var celebrityService = new CelebrityService(data, MapperMock.Instance);

            var result = celebrityService.AllAdmin();

            Assert.NotNull(result);
            Assert.Single(result.Celebrities);
        }

        [Fact]
        public void ChangeStatusOfCelebrityWorksAsExpectedWithValidData()
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

            var result = celebrityService.ChangeStatus(celebrity.Id);

            Assert.True(result);
        }

        [Fact]
        public void ChangeStatusOfCelebrityReturnFalseIfCelebrityIsDeleted()
        {
            var celebrity = new Celebrity
            {
                Id = 1,
                ImageUrl = null,
                Description = "Test test test test test",
                IsDeleted = true,
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

            var result = celebrityService.ChangeStatus(celebrity.Id);

            Assert.False(result);
        }

        [Fact]
        public void IsByPartnerReturnsTrueIfCelebrityIsAddedByThisPartner()
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

            var result = celebrityService.IsByPartner(1, 1);

            Assert.True(result);
        }

        [Fact]
        public void IsByPartnerReturnsFalseIfCelebrityIsNotAddedByThisPartner()
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

            var result = celebrityService.IsByPartner(1, 2);

            Assert.False(result);
        }
    }
}
