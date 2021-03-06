namespace DreamIsland.Models.Islands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using DreamIsland.Models.Contracts;
    using DreamIsland.Services.Island.Models;

    using static Data.DataConstants.Island;

    public class IslandAddFormModel : IFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The island name should be between {2} and {1} characters")]
        public string Name { get; set; }

        [Required]
        [StringLength(LocationMaxLength, MinimumLength = LocationMinLength, ErrorMessage = "The location should be between {2} and {1} characters")]
        public string Location { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The location should be between {2} and {1} characters")]
        public string Description { get; set; }

        [Display(Name = "Size in square km")]
        [Range(double.Epsilon, double.MaxValue, ErrorMessage = "The area should be bigger than 0 km")]
        public double SizeInSquareKm { get; set; }

        [Display(Name = "Price in USD")]
        public decimal? Price { get; set; }

        [Display(Name = "Upload photo")]
        public IFormFile CoverPhoto { get; set; }

        [Display(Name = "Population")]
        public int PopulationSizeId { get; set; }

        [Display(Name = "Region")]
        public int IslandRegionId { get; set; }

        public IEnumerable<IslandRegionServiceModel> IslandRegions { get; set; }
        public IEnumerable<IslandPopulationSizeServiceModel> PopulationSizes { get; set; }
    }
}
