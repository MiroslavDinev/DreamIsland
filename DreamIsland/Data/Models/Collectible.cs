namespace DreamIsland.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using DreamIsland.Data.Enums;

    using static DataConstants.Collectible;

    public class Collectible
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public bool IsPublic { get; set; }

        public RarityLevel RarityLevel { get; set; }

        public int PartnerId { get; set; }

        public Partner Partner { get; set; }
    }
}
