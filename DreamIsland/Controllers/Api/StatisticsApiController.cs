namespace DreamIsland.Controllers.Api
{
    using System;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;

    using DreamIsland.Services.Statistic;
    using DreamIsland.Models.Api.Statistics;

    using static WebConstants;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;
        private readonly IMemoryCache cache;

        public StatisticsApiController(IStatisticsService statisticsService, IMemoryCache cache)
        {
            this.statisticsService = statisticsService;
            this.cache = cache;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var statistics = this.cache.Get<StatisticsResponseModel>(StatisticsCacheKey);

            if(statistics == null)
            {
                statistics = this.statisticsService.GetStatistics();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(15));

                this.cache.Set(StatisticsCacheKey, statistics, cacheOptions);
            }           

            return statistics;
        }
    }
}
