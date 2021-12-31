namespace DreamIsland.Models
{
    public abstract class BaseQueryModel
    {
        public const int ItemsPerPage = 3;

        public int TotalItems { get; set; }

        public int CurrentPage { get; set; } = 1;
    }
}
