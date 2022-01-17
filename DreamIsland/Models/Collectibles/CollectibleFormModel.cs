namespace DreamIsland.Models.Collectibles
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using DreamIsland.Data.Enums;
    using static Data.DataConstants.Collectible;

    public class CollectibleFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The name should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The description should be between {2} and {1} symbols")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Upload photo")]
        public IFormFile CoverPhoto { get; set; }
        public string ImageUrl { get; set; }

        [Display(Name = "Rarity Level")]
        public RarityLevel RarityLevel { get; set; }

        public int PartnerId { get; set; }
    }
}
