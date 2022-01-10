namespace DreamIsland.Areas.Admin.Models.Collectible
{
    using System.Collections.Generic;

    using DreamIsland.Models.Collectibles;

    public class AllAdminCollectiblesQueryModel : BaseAdminQueryModel
    {
        public IEnumerable<CollectibleListingViewModel> Collectibles { get; set; }
    }
}
