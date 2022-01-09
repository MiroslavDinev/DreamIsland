namespace DreamIsland.Models.Islands
{
    public class IslandListingViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public double SizeInSquareKm { get; set; }

        public decimal? Price { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPublic { get; set; }
    }
}
