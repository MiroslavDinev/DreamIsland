namespace DreamIsland.Models.Contracts
{
    using Microsoft.AspNetCore.Http;
    public interface IFormModel
    {
        string Description { get; set; }

        IFormFile CoverPhoto { get; set; }
    }
}
