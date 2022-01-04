namespace DreamIsland.Services.Collectible.Models
{
    using DreamIsland.Data.Enums;

    public class CollectibleDetailsServiceModel : BaseServiceModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public RarityLevel RarityLevel { get; set; }
    }
}
