namespace DreamIsland.Services.Collectible
{
    using System.Threading.Tasks;

    using DreamIsland.Data.Enums;
    using DreamIsland.Models.Collectibles;

    public interface ICollectibleService
    {
        Task<int> AddAsync(string name, string description, string imageUrl, RarityLevel rarityLevel, int partnerId);

        AllCollectiblesQueryModel All(string rarityLevel = null, string searchTerm = null);
    }
}
