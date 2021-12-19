namespace DreamIsland.Services.Island
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Data;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Services.Island.Models;

    public class IslandService : IIslandService
    {
        private readonly DreamIslandDbContext data;

        public IslandService(DreamIslandDbContext data)
        {
            this.data = data;
        }

        public async Task<int> AddAsync(string name, string location, string description, double sizeInSquareKm, 
            decimal? price, string imageUrl, int populationSizeId, int islandRegionId, int partnerId)
        {
            var island = new Island
            {
                Name = name,
                Location = location,
                Description = description,
                SizeInSquareKm = sizeInSquareKm,
                Price = price,
                ImageUrl = imageUrl,
                PopulationSizeId = populationSizeId,
                IslandRegionId = islandRegionId,
                PartnerId = partnerId
            };

            await this.data.AddAsync(island);
            await this.data.SaveChangesAsync();

            return island.Id;
        }

        public IEnumerable<IslandPopulationSizeServiceModel> GetPopulationSizes()
        {
            var populationSizes = this.data
                .PopulationSizes
                .Select(x => new IslandPopulationSizeServiceModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return populationSizes;
        }

        public IEnumerable<IslandRegionServiceModel> GetRegions()
        {
            var islandRegions = this.data
                .IslandRegions
                .Select(x => new IslandRegionServiceModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            return islandRegions;
        }

        public bool PopulationSizeExists(int populationSizeId)
        {
            var populationSizeExists = this.data
                .PopulationSizes
                .Any(x => x.Id == populationSizeId);

            return populationSizeExists;
        }

        public bool RegionExists(int islandRegionId)
        {
            var regionExists = this.data
                .IslandRegions
                .Any(x => x.Id == islandRegionId);

            return regionExists;
        }
    }
}
