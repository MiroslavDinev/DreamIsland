namespace DreamIsland.Services.Collectible
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data.Models;
    using DreamIsland.Data;
    using DreamIsland.Data.Enums;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Infrastructure;
    using DreamIsland.Services.Collectible.Models;
    using DreamIsland.Areas.Admin.Models.Collectible;

    public class CollectibleService : ICollectibleService
    {
        private readonly DreamIslandDbContext data;
        private readonly IMapper mapper;

        public CollectibleService(DreamIslandDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }
        public async Task<int> AddAsync(string name, string description, string imageUrl, RarityLevel rarityLevel, int partnerId)
        {
            var collectible = new Collectible
            {
                Name = name,
                Description = description,
                ImageUrl = imageUrl,
                RarityLevel = rarityLevel,
                PartnerId = partnerId,
                IsPublic = false
            };

            await this.data.Collectibles.AddAsync(collectible);
            await this.data.SaveChangesAsync();

            return collectible.Id;
        }

        public AllCollectiblesQueryModel All(string rarityLevel = null, string searchTerm = null, int currentPage = 1)
        {
            var collectiblesQuery = this.data
                .Collectibles
                .Where(c=> c.IsPublic && !c.IsDeleted);

            if (!string.IsNullOrEmpty(rarityLevel))
            {
                rarityLevel = rarityLevel.Replace(" ", "");

                Enum.TryParse(rarityLevel, out RarityLevel rarityEnumLevel);

                collectiblesQuery = collectiblesQuery
                    .Where(x => x.RarityLevel == rarityEnumLevel);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                collectiblesQuery = collectiblesQuery
                    .Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var totalCollectibles = collectiblesQuery.Count();

            var collectibles = this.GetCollectibles(collectiblesQuery
                .OrderByDescending(x => x.Id)
                .Skip((currentPage - 1) * AllCollectiblesQueryModel.ItemsPerPage)
                .Take(AllCollectiblesQueryModel.ItemsPerPage));
                

            var rarityLevels = this.data
                .Collectibles
                .Select(x => EnumValuesExtension.GetDescriptionFromEnum(x.RarityLevel))
                .Distinct()
                .ToList();

            var collectible = new AllCollectiblesQueryModel
            {
                Collectibles = collectibles,
                RarityLevels = rarityLevels,
                TotalItems = totalCollectibles,
                CurrentPage = currentPage,
                SearchTerm = searchTerm,
                RarityLevel = rarityLevel
            };

            return collectible;
        }

        public AllAdminCollectiblesQueryModel AllAdmin(int currentPage = 1)
        {
            var collectiblesQuery = this.data
                .Collectibles
                .Where(c => !c.IsDeleted);

            var totalCollectibles = collectiblesQuery.Count();

            var collectibles = this.GetCollectibles(collectiblesQuery
                .OrderBy(x => x.Id)
                .Skip((currentPage - 1) * AllAdminCollectiblesQueryModel.ItemsPerPage)
                .Take(AllAdminCollectiblesQueryModel.ItemsPerPage));

            var collectible = new AllAdminCollectiblesQueryModel
            {
                Collectibles = collectibles,
                CurrentPage = currentPage,
                TotalItems = totalCollectibles
            };

            return collectible;
        }

        public bool ChangeStatus(int collectibleId)
        {
            var collectible = this.data.Collectibles.Find(collectibleId);

            if(collectible == null)
            {
                return false;
            }
            else if (collectible.IsDeleted)
            {
                return false;
            }

            collectible.IsPublic = !collectible.IsPublic;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int collectibleId)
        {
            var collectible = this.data.Collectibles.Find(collectibleId);

            if(collectible == null)
            {
                return false;
            }
            else if (collectible.IsDeleted)
            {
                return false;
            }

            collectible.IsDeleted = true;

            this.data.SaveChanges();

            return true;
        }

        public CollectibleDetailsServiceModel Details(int collectibleId)
        {
            var collectible = this.data
                .Collectibles
                .Where(x => x.Id == collectibleId && !x.IsDeleted)
                .ProjectTo<CollectibleDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

            return collectible;
        }

        public async Task<bool> EditAsync(int collectibleId, string name, string description, string imageUrl, RarityLevel rarityLevel, bool isPublic)
        {
            var collectible = this.data.Collectibles.Find(collectibleId);

            if(collectible == null)
            {
                return false;
            }
            else if (collectible.IsDeleted)
            {
                return false;
            }

            collectible.Name = name;
            collectible.Description = description;
            collectible.ImageUrl = imageUrl;
            collectible.RarityLevel = rarityLevel;
            collectible.IsPublic = isPublic;

            await this.data.SaveChangesAsync();

            return true;
        }

        public IEnumerable<CollectibleListingViewModel> GetCollectiblesByPartner(string userId)
        {
            var collectibles = this.GetCollectibles(this.data
                .Collectibles
                .Where(x => x.Partner.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.Id));

            return collectibles;
        }

        public bool IsByPartner(int collectibleId, int partnerId)
        {
            var isByPartner = this.data
                .Collectibles
                .Any(x => x.Id == collectibleId && x.PartnerId == partnerId);

            return isByPartner;
        }

        private IEnumerable<CollectibleListingViewModel> GetCollectibles(IQueryable<Collectible> collectiblesQuery)
        {
            var collectibles = collectiblesQuery
                .Select(x => new CollectibleListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    RarityLevel = EnumValuesExtension.GetDescriptionFromEnum(x.RarityLevel),
                    IsPublic = x.IsPublic
                })
                .ToList();

            return collectibles;

        }
    }
}
