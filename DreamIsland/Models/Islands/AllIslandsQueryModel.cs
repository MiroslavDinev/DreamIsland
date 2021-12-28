namespace DreamIsland.Models.Islands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllIslandsQueryModel
    {
        [Display(Name = "Search by text")]
        public string SearchTerm { get; init; }
        public string Region { get; set; }
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<IslandListingViewModel> Islands { get; set; }
    }
}
