namespace DreamIsland.Tests.Services
{
    using Xunit;

    using DreamIsland.Tests.Mock;
    using DreamIsland.Data.Models.Celebrities;
    using DreamIsland.Data.Models.Collectibles;
    using DreamIsland.Data.Enums;
    using DreamIsland.Services.Statistic;

    public class StatisticsServiceTests
    {
        [Fact]
        public void TestStatisticsServiceReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;
            data.Celebrities.Add(new Celebrity
            {
                Name = "Test",
                IsPublic = true,
                IsDeleted = false,
                Description = "Test Description",
                Age = 30,
                ImageUrl = null,
                Occupation = "Actor"
            });

            data.Collectibles.Add(new Collectible
            {
                Name = "Test Sword",
                ImageUrl = null,
                IsPublic = true,
                IsDeleted = false,
                Description = "Test Description",
                RarityLevel = RarityLevel.Rare
            });

            data.SaveChanges();

            var statisticsService = new StatisticsService(data);

            var result = statisticsService.GetStatistics();

            Assert.NotNull(result);
            Assert.Equal(1, result.TotalCollectibles);
            Assert.Equal(1, result.TotalCelebrities);
            Assert.Equal(0, result.TotalCars);
            Assert.Equal(0, result.TotalIslands);

        }
    }
}
