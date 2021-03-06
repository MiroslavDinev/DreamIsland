namespace DreamIsland.Models.Islands
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DreamIsland.Models.Islands.Enums;

    public class AllIslandsQueryModel : BaseQueryModel
    {
        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }
        public string Region { get; set; }

        [Display(Name = "Sort by")]
        public IslandSorting IslandSorting { get; set; }
        public IEnumerable<string> Regions { get; set; }
        public IEnumerable<IslandListingViewModel> Islands { get; set; }
    }
}
