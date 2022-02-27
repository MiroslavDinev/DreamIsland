namespace DreamIsland.Tests.Mock
{
    using Moq;

    using DreamIsland.Services.Statistic;
    using Models.Api.Statistics;

    public static class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();
                statisticsServiceMock.Setup(s => s.GetStatistics())
                    .Returns(new StatisticsResponseModel
                    {
                        TotalCars = 5,
                        TotalCelebrities = 10,
                        TotalCollectibles = 15,
                        TotalIslands = 20
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}
