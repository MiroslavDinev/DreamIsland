namespace DreamIsland.Services.Island
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Models.Islands;
    using DreamIsland.Services.Island.Models;
    using DreamIsland.Models.Islands.Enums;
    using DreamIsland.Areas.Admin.Models.Island;

    public interface IIslandService
    {
        IEnumerable<IslandPopulationSizeServiceModel> GetPopulationSizes();
        IEnumerable<IslandRegionServiceModel> GetRegions();

        AllIslandsQueryModel All(string region = null, string searchTerm = null, IslandSorting islandSorting = IslandSorting.DateAdded, int currentPage = 1);
        AllAdminIslandsQueryModel AllAdmin(int currentPage = 1);

        Task<bool> ChangeStatus(int islandId);

        Task<int> AddAsync(string name, string location, string description, double sizeInSquareKm, 
            decimal? price, string imageUrl, int populationSizeId, int islandRegionId, int partnerId);

        Task<bool> EditAsync(int islandId, string name, string location, string description, double sizeInSquareKm,
            decimal? price, string imageUrl, int populationSizeId, int islandRegionId, bool isPublic);

        Task<bool> Delete(int islandId);

        bool PopulationSizeExists(int populationSizeId);
        bool RegionExists(int islandRegionId);
        IEnumerable<IslandListingViewModel> GetIslandsByPartner(string userId);
        IEnumerable<LatestIslandsServiceModel> LatestIslands();

        IslandDetailsServiceModel Details(int islandId);

        bool IsByPartner(int islandId, int partnerId);
    }
}
