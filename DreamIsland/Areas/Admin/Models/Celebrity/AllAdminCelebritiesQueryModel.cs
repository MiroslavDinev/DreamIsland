namespace DreamIsland.Areas.Admin.Models.Celebrity
{
    using System.Collections.Generic;

    using DreamIsland.Models.Celebrities;

    public class AllAdminCelebritiesQueryModel : BaseAdminQueryModel
    {
        public IEnumerable<CelebrityListingViewModel> Celebrities { get; set; }
    }
}
