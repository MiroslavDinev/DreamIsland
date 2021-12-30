namespace DreamIsland.Models.Celebrities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllCelebritiesQueryModel
    {
        [Display(Name = "Search by name")]
        public string SearchTerm { get; set; }
        public string Occupation { get; set; }
        public IEnumerable<string> Occupations { get; set; }
        public IEnumerable<CelebrityListingViewModel> Celebrities { get; set; }
    }
}
