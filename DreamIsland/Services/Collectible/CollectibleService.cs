namespace DreamIsland.Services.Collectible
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Data.Models;
    using DreamIsland.Data;
    using DreamIsland.Data.Enums;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Infrastructure;

    public class CollectibleService : ICollectibleService
    {
        private readonly DreamIslandDbContext data;

        public CollectibleService(DreamIslandDbContext data)
        {
            this.data = data;
        }
        public async Task<int> AddAsync(string name, string description, string imageUrl, RarityLevel rarityLevel, int partnerId)
        {
            var collectible = new Collectible
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                RarityLevel = rarityLevel,
                PartnerId = partnerId
            };

            await this.data.Collectibles.AddAsync(collectible);
            await this.data.SaveChangesAsync();

            return collectible.Id;
        }

        public IEnumerable<CollectibleListingViewModel> All()
        {
            var collectibles = this.data
                .Collectibles
                .OrderByDescending(x => x.Id)
                .Select(x => new CollectibleListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    RarityLevel = EnumValuesExtension.GetDescriptionFromEnum(x.RarityLevel)
                })
                .ToList();

            return collectibles;
        }
    }
}
