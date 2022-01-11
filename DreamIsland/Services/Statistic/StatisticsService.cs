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
                TotalIslands = this.data.Islands.Count(i=> i.IsPublic && !i.IsDeleted),
                TotalCars = this.data.Cars.Count(c=> c.IsPublic && !c.IsDeleted),
                TotalCelebrities = this.data.Celebrities.Count(c=> c.IsPublic && !c.IsDeleted),
                TotalCollectibles = this.data.Collectibles.Count(c=> c.IsPublic && !c.IsDeleted)
            };

            return statistics;
        }
    }
}
