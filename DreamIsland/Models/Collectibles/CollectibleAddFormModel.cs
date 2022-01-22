namespace DreamIsland.Models.Collectibles
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using DreamIsland.Data.Enums;
    using static Data.DataConstants.Collectible;
    using DreamIsland.Models.Contracts;

    public class CollectibleAddFormModel : IFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The name should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The description should be between {2} and {1} symbols")]
        public string Description { get; set; }

        [Display(Name = "Upload photo")]
        public IFormFile CoverPhoto { get; set; }

        [Display(Name = "Rarity Level")]
        public RarityLevel RarityLevel { get; set; }

        public int PartnerId { get; set; }
    }
}
