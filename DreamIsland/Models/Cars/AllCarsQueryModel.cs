namespace DreamIsland.Models.Cars
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using DreamIsland.Models.Cars.Enums;

    public class AllCarsQueryModel
    {
        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        [Display(Name = "Sort by")]
        public CarsSorting CarsSorting { get; set; }

        public IEnumerable<string> Brands { get; set; }
        public IEnumerable<CarListingViewModel> Cars { get; set; }
    }
}
