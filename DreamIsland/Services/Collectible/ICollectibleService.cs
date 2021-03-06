namespace DreamIsland.Services.Collectible
{
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Data.Enums;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Areas.Admin.Models.Collectible;
    using DreamIsland.Services.Collectible.Models;

    public interface ICollectibleService
    {
        Task<int> AddAsync(string name, string description, string imageUrl, RarityLevel rarityLevel, int partnerId);

        Task<bool> EditAsync(int collectibleId, string name, string description, string imageUrl, RarityLevel rarityLevel, bool isPublic);

        AllCollectiblesQueryModel All(string rarityLevel = null, string searchTerm = null, int currentPage = 1);

        AllAdminCollectiblesQueryModel AllAdmin(int currentPage = 1);

        Task<bool> ChangeStatus(int collectibleId);
        Task<bool> Delete(int collectibleId);

        IEnumerable<CollectibleListingViewModel> GetCollectiblesByPartner(string userId);

        CollectibleDetailsServiceModel Details(int collectibleId);

        bool IsByPartner(int collectibleId, int partnerId);
    }
}
