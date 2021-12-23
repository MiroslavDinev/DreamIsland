namespace DreamIsland.Services.Statistic
{
    using System.Linq;

    using DreamIsland.Data;
    using DreamIsland.Models.Api.Statistics;

    public class StatisticsService : IStatisticsService
    {
        private readonly DreamIslandDbContext data;

        public StatisticsService(DreamIslandDbContext data)
        {
            this.data = data;
        }
        public StatisticsResponseModel GetStatistics()
        {
            var statistics = new StatisticsResponseModel
            {
                TotalIslands = this.data.Islands.Count(),
                TotalCars = this.data.Cars.Count(),
                TotalCelebrities = this.data.Celebrities.Count(),
                TotalCollectibles = this.data.Collectibles.Count()
            };

            return statistics;
        }
    }
}
