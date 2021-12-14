namespace DreamIsland.Models.Cars
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Vehicle;

    public class AddCarFormModel
    {
        [Required]
        [StringLength(BrandMaxLength, MinimumLength = BrandMinLength, ErrorMessage = "The car brand should be between {2} and {1} symbols")]
        public string Brand { get; set; }

        [Required]
        [StringLength(ModelMaxLength, MinimumLength = ModelMinLength, ErrorMessage = "The car model should be between {2} and {1} symbols")]
        public string Model { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, 
            ErrorMessage = "The car description should be between {2} and {1} symbols")]
        public string Description { get; set; }

        [Required]
        [Url]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
        public int Year { get; set; }

        public bool HasRemoteStart { get; set; }

        public bool HasRemoteControlParking { get; set; }

        public bool HasSeatMassager { get; set; }
    }
}
