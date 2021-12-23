namespace DreamIsland.Models.Home
{
    using System.Collections.Generic;

    using DreamIsland.Models.Islands;
    public class IndexViewModel
    {
        public int TotalIslands { get; set; }
        public int TotalCars { get; set; }
        public int TotalCelebrities { get; set; }
        public int TotalCollectibles { get; set; }
        public IList<IslandListingViewModel> Islands { get; set; }
    }
}
