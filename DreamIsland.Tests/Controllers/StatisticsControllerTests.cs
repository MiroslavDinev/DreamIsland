namespace DreamIsland.Tests.Controllers
{
    using Xunit;

    using DreamIsland.Controllers.Api;
    using DreamIsland.Models.Api.Statistics;
    using DreamIsland.Tests.Mock;

    public class StatisticsControllerTests
    {
        [Fact]
        public void StatisticsReturnTheCorrectResult()
        {
            var statisticsController = new StatisticsApiController(StatisticsServiceMock.Instance, MockMemoryCacheServiceMock.GetMemoryCache(new StatisticsResponseModel
            {
                TotalCars =  5,
                TotalCelebrities = 10,
                TotalCollectibles = 15,
                TotalIslands = 20
            }));

            var result = statisticsController.GetStatistics();

            Assert.NotNull(result);
            Assert.Equal(5, result.TotalCars);
            Assert.Equal(10, result.TotalCelebrities);
            Assert.Equal(15, result.TotalCollectibles);
            Assert.Equal(20, result.TotalIslands);
            
        }
        
    }
}
