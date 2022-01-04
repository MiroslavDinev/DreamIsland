namespace DreamIsland.Services.Collectible
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DreamIsland.Data.Enums;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Services.Collectible.Models;

    public interface ICollectibleService
    {
        Task<int> AddAsync(string name, string description, string imageUrl, RarityLevel rarityLevel, int partnerId);

        Task<bool> EditAsync(int collectibleId, string name, string description, string imageUrl, RarityLevel rarityLevel);

        AllCollectiblesQueryModel All(string rarityLevel = null, string searchTerm = null, int currentPage = 1);

        IEnumerable<CollectibleListingViewModel> GetCollectiblesByPartner(string userId);

        CollectibleDetailsServiceModel Details(int collectibleId);

        bool IsByPartner(int collectibleId, int partnerId);
    }
}
