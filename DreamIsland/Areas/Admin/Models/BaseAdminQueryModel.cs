namespace DreamIsland.Areas.Admin.Models
{
    public class BaseAdminQueryModel
    {
        public const int ItemsPerPage = 10;

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; } = 1;
    }
}
