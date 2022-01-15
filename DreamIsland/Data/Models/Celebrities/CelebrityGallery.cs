namespace DreamIsland.Data.Models.Celebrities
{
    public class CelebrityGallery : BaseGalleryModel
    {
        public int CelebrityId { get; set; }

        public Celebrity Celebrity { get; set; }
    }
}
