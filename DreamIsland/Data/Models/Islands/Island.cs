namespace DreamIsland.Data.Models.Islands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using DreamIsland.Data.Models.Vehicles;

    using static DataConstants.Island;

    public class Island
    {
        public Island()
        {
            this.Cars = new HashSet<Car>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public double SizeInSquareKm { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int PartnerId { get; set; }

        public Partner Partner { get; set; }

        public int PopulationSizeId { get; set; }
        public PopulationSize PopulationSize { get; set; }

        public int IslandRegionId { get; set; }

        public IslandRegion IslandRegion { get; set; }

        public IEnumerable<Car> Cars { get; set; }
    }
}
