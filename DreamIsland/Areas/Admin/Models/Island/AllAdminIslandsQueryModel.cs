namespace DreamIsland.Areas.Admin.Models.Island
{
    using System.Collections.Generic;

    using DreamIsland.Models.Islands;

    public class AllAdminIslandsQueryModel : BaseAdminQueryModel
    {
        public IEnumerable<IslandListingViewModel> Islands { get; set; }
    }
}
