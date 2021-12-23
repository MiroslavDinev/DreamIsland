namespace DreamIsland.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Models.Api.Statistics;
    using DreamIsland.Services.Statistic;

    [ApiController]
    [Route("api/statistics")]
    public class StatisticsApiController : ControllerBase
    {
        private readonly IStatisticsService statisticsService;

        public StatisticsApiController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpGet]
        public StatisticsResponseModel GetStatistics()
        {
            var statistics = this.statisticsService.GetStatistics();

            return statistics;
        }
    }
}
