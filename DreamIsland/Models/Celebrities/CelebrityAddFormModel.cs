namespace DreamIsland.Models.Celebrities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using DreamIsland.Models.Contracts;

    using static Data.DataConstants.Celebrity;

    public class CelebrityAddFormModel : IFormModel
    {

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The name should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [StringLength(OccupationMaxLength, MinimumLength = OccupationMinLength, ErrorMessage = "The occupation should be between {2} and {1} symbols")]
        public string Occupation { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The description should be between {2} and {1} symbols")]
        public string Description { get; set; }

        [Display(Name = "Upload photo")]
        public IFormFile CoverPhoto { get; set; }

        [Range(MinAge, MaxAge)]
        public int? Age { get; set; }
    }
}
