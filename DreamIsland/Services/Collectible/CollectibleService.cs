﻿namespace DreamIsland.Services.Collectible
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

        public AllCollectiblesQueryModel All(string rarityLevel = null, string searchTerm = null)
        {
            var collectiblesQuery = this.data
                .Collectibles
                .AsQueryable();

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

            var collectibles = collectiblesQuery
                .OrderByDescending(x => x.Id)
                .Select(x => new CollectibleListingViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.ImageUrl,
                    RarityLevel = EnumValuesExtension.GetDescriptionFromEnum(x.RarityLevel)
                })
                .ToList();

            var rarityLevels = this.data
                .Collectibles
                .Select(x => EnumValuesExtension.GetDescriptionFromEnum(x.RarityLevel))
                .ToList();

            var collectible = new AllCollectiblesQueryModel
            {
                Collectibles = collectibles,
                RarityLevels = rarityLevels
            };

            return collectible;
        }
    }
}
