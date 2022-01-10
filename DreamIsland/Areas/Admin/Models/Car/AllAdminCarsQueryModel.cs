namespace DreamIsland.Areas.Admin.Models.Car
{
    using System.Collections.Generic;

    using DreamIsland.Models.Cars;

    public class AllAdminCarsQueryModel : BaseAdminQueryModel
    {
        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
