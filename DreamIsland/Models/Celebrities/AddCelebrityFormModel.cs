namespace DreamIsland.Models.Celebrities
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Celebrity;

    public class AddCelebrityFormModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = "The name should be between {2} and {1} symbols")]
        public string Name { get; set; }

        [Required]
        [StringLength(OccupationMaxLength, MinimumLength = OccupationMinLength, ErrorMessage = "The occupation should be between {2} and {1} symbols")]
        public string Occupation { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The description should be between {2} and {1} symbols")]
        public string Description { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Range(MinAge, MaxAge)]
        public int Age { get; set; }
    }
}
