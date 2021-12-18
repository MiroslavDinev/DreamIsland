namespace DreamIsland.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DreamIsland.Data.Models.Vehicles;
    using static DataConstants.Partner;

    public class Partner
    {
        public Partner()
        {
            this.Cars = new HashSet<Car>();
            this.Celebrities = new HashSet<Celebrity>();
            this.Collectibles = new HashSet<Collectible>();
            this.Islands = new HashSet<Island>();
        }
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; set; }
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Celebrity> Celebrities { get; set; }
        public IEnumerable<Collectible> Collectibles { get; set; }
        public IEnumerable<Island> Islands { get; set; }
    }
}
