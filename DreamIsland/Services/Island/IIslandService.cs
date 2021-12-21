namespace DreamIsland.Services.Island
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Models.Islands;
    using DreamIsland.Services.Island.Models;

    public interface IIslandService
    {
        IEnumerable<IslandPopulationSizeServiceModel> GetPopulationSizes();
        IEnumerable<IslandRegionServiceModel> GetRegions();

        IEnumerable<IslandListingViewModel> All();

        Task<int> AddAsync(string name, string location, string description, double sizeInSquareKm, 
            decimal? price, string imageUrl, int populationSizeId, int islandRegionId, int partnerId);

        bool PopulationSizeExists(int populationSizeId);
        bool RegionExists(int islandRegionId);
    }
}
