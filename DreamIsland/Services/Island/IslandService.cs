namespace DreamIsland.Services.Island
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using DreamIsland.Data;
    using DreamIsland.Models.Islands;
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

        public AllIslandsQueryModel All(string region = null, string searchTerm = null)
        {
            var islandsQuery = this.data
                .Islands
                .AsQueryable();

            if(!string.IsNullOrEmpty(region))
            {
                islandsQuery = islandsQuery
                    .Where(x => x.IslandRegion.Name == region);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                islandsQuery = islandsQuery
                    .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    x.Location.ToLower().Contains(searchTerm.ToLower()));
            }

            var islands = islandsQuery
                .OrderByDescending(x=> x.Id)
                .Select(x => new IslandListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Location = x.Location,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    SizeInSquareKm = x.SizeInSquareKm
                })
                .ToList();

            var regions = this.data
                .IslandRegions
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var island = new AllIslandsQueryModel
            {
                Islands = islands,
                Regions = regions
            };

            return island;
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
