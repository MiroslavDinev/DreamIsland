namespace DreamIsland.Services.Celebrity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DreamIsland.Models.Celebrities;

    public interface ICelebrityService
    {
        Task<int> Add(string name, string occupation, string description, string imageUrl, int age, int partnerId);

        IEnumerable<CelebrityListingViewModel> All();
    }
}
