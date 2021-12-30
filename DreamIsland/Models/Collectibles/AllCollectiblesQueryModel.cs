namespace DreamIsland.Models.Collectibles
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllCollectiblesQueryModel
    {
        [Display(Name = "Search by name")]
        public string SearchTerm { get; set; }

        [Display(Name = "Rarity level")]
        public string RarityLevel { get; set; }

        public IEnumerable<string> RarityLevels { get; set; }
        public IEnumerable<CollectibleListingViewModel> Collectibles { get; set; }
    }
}
