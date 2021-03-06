namespace DreamIsland.Services.Island
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using DreamIsland.Data;
    using DreamIsland.Models.Islands;
    using DreamIsland.Data.Models.Islands;
    using DreamIsland.Services.Island.Models;
    using DreamIsland.Models.Islands.Enums;
    using DreamIsland.Areas.Admin.Models.Island;

    public class IslandService : IIslandService
    {
        private readonly DreamIslandDbContext data;
        private readonly IMapper mapper;

        public IslandService(DreamIslandDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
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
                PartnerId = partnerId,
                IsPublic = false
            };

            await this.data.AddAsync(island);
            await this.data.SaveChangesAsync();

            return island.Id;
        }

        public AllAdminIslandsQueryModel AllAdmin(int currentPage = 1)
        {
            var islandsQuery = this.data
                .Islands
                .Where(x => !x.IsDeleted);

            var totalIslands = islandsQuery.Count();

            var islands = this.GetIslands(islandsQuery
                .OrderBy(x=> x.Id)
                .Skip((currentPage - 1) * AllAdminIslandsQueryModel.ItemsPerPage)
                .Take(AllAdminIslandsQueryModel.ItemsPerPage));

            var island = new AllAdminIslandsQueryModel
            {
                CurrentPage = currentPage,
                Islands = islands,
                TotalItems = totalIslands
            };

            return island;
        }

        public AllIslandsQueryModel All(string region = null, string searchTerm = null, IslandSorting islandSorting = IslandSorting.DateAdded, int currentPage = 1)
        {
            var islandsQuery = this.data
                .Islands
                .Where(i => i.IsPublic && !i.IsDeleted);

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

            islandsQuery = islandSorting switch
            {
                IslandSorting.PriceAscending => islandsQuery.OrderBy(x=> x.Price),
                IslandSorting.PriceDescending => islandsQuery.OrderByDescending(x=> x.Price),
                IslandSorting.AreaAscending => islandsQuery.OrderBy(x=> x.SizeInSquareKm),
                IslandSorting.AreaDescending => islandsQuery.OrderByDescending(x=> x.SizeInSquareKm),
                IslandSorting.DateAdded or _ => islandsQuery.OrderByDescending(x=> x.Id)
            };

            var totalIslands = islandsQuery.Count();

            var islands = this.GetIslands(islandsQuery
                .Skip((currentPage - 1) * AllIslandsQueryModel.ItemsPerPage)
                .Take(AllIslandsQueryModel.ItemsPerPage));

            var regions = this.data
                .IslandRegions
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var island = new AllIslandsQueryModel
            {
                Islands = islands,
                Regions = regions,
                TotalItems = totalIslands,
                CurrentPage = currentPage,
                IslandSorting = islandSorting,
                SearchTerm = searchTerm,
                Region = region
            };

            return island;
        }

        public IslandDetailsServiceModel Details(int islandId)
        {
            var island = this.data.Islands
                .Where(x => x.Id == islandId && !x.IsDeleted)
                .ProjectTo<IslandDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

            return island;
        }

        public IEnumerable<IslandListingViewModel> GetIslandsByPartner(string userId)
        {
            var islands = this.GetIslands(this.data
                .Islands
                .Where(x => x.Partner.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.Id));

            return islands;
        }

        public IEnumerable<IslandPopulationSizeServiceModel> GetPopulationSizes()
        {
            var populationSizes = this.data
                .PopulationSizes
                .ProjectTo<IslandPopulationSizeServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return populationSizes;
        }

        public IEnumerable<IslandRegionServiceModel> GetRegions()
        {
            var islandRegions = this.data
                .IslandRegions
                .ProjectTo<IslandRegionServiceModel>(this.mapper.ConfigurationProvider)
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

        public bool IsByPartner(int islandId, int partnerId)
        {
            var isByPartner = this.data
                .Islands
                .Any(x => x.Id == islandId && x.PartnerId == partnerId);

            return isByPartner;
        }

        private IEnumerable<IslandListingViewModel> GetIslands (IQueryable islandsQuery)
        {
            var islands = islandsQuery
                .ProjectTo<IslandListingViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return islands;
        }

        public async Task<bool> EditAsync(int islandId, string name, string location, string description, 
            double sizeInSquareKm, decimal? price, string imageUrl, int populationSizeId, int islandRegionId, bool isPublic)
        {
            var island = this.data.Islands.Find(islandId);

            if(island == null)
            {
                return false;
            }
            else if (island.IsDeleted)
            {
                return false;
            }

            island.Name = name;
            island.Location = location;
            island.Description = description;
            island.SizeInSquareKm = sizeInSquareKm;
            island.Price = price;
            island.ImageUrl = imageUrl;
            island.PopulationSizeId = populationSizeId;
            island.IslandRegionId = islandRegionId;
            island.IsPublic = isPublic;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int islandId)
        {
            var island = this.data.Islands.Find(islandId);

            if(island == null)
            {
                return false;
            }
            else if (island.IsDeleted)
            {
                return false;
            }

            island.IsDeleted = true;

            await this.data.SaveChangesAsync();

            return true;
        }

        public IEnumerable<LatestIslandsServiceModel> LatestIslands()
        {
            var latestIslands = this.data.Islands
                .Where(x=> x.IsPublic && !x.IsDeleted)
                .OrderByDescending(x=> x.Id)
                .ProjectTo<LatestIslandsServiceModel>(this.mapper.ConfigurationProvider)
                .Take(3)
                .ToList();

            return latestIslands;
        }

        public async Task<bool> ChangeStatus(int islandId)
        {
            var island = this.data.Islands.Find(islandId);

            if (island==null)
            {
                return false;
            }
            else if (island.IsDeleted)
            {
                return false;
            }

            island.IsPublic = !island.IsPublic;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
