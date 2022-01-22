namespace DreamIsland.Data.Models.Vehicles
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Vehicle;
    public abstract class Vehicle : BaseDataModel
    {
        [Required]
        [MaxLength(BrandMaxLength)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }
        public int Year { get; set; }
    }
}
