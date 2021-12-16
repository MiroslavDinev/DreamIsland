namespace DreamIsland.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

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

        public double SizeInSquareKm { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public int PartnerId { get; set; }

        public Partner Partner { get; set; }

        public IEnumerable<Car> Cars { get; set; }
    }
}
