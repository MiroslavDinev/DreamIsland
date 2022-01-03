namespace DreamIsland.Services.Island.Models
{
    using System.Collections.Generic;

    using DreamIsland.Models.Cars;

    public class IslandDetailsServiceModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public double SizeInSquareKm { get; set; }
        public decimal? Price { get; set; }
        public string ImageUrl { get; set; }
        public int IslandRegionId { get; set; }
        public string IslandRegionName { get; set; }
        public int PopulationSizeId { get; set; }
        public string PopulationSizeName { get; set; }
        public int PartnerId { get; set; }
        public string PartnerName { get; set; }
        public string UserId { get; set; }
    }
}
