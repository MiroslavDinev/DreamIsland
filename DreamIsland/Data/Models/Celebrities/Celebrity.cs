namespace DreamIsland.Data.Models.Celebrities
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Celebrity;

    public class Celebrity : BaseDataModel
    {

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(OccupationMaxLength)]
        public string Occupation { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int? Age { get; set; }

        public int PartnerId { get; set; }
        public Partner Partner { get; set; }

    }
}
