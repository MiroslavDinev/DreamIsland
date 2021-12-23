namespace DreamIsland.Services.Statistic
{
    using DreamIsland.Models.Api.Statistics;

    public interface IStatisticsService
    {
        StatisticsResponseModel GetStatistics();
    }
}
