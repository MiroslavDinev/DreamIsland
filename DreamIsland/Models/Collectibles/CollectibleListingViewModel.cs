namespace DreamIsland.Models.Collectibles
{
    public class CollectibleListingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string RarityLevel { get; set; }
        public bool IsPublic { get; set; }
    }
}
