namespace DreamIsland.Services.Celebrity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DreamIsland.Models.Celebrities;

    public interface ICelebrityService
    {
        Task<int> AddAsync(string name, string occupation, string description, string imageUrl, int? age, int partnerId);

        AllCelebritiesQueryModel All(string occupation = null, string searchTerm = null, int currentPage = 1);

        IEnumerable<CelebrityListingViewModel> GetCelebritiesByPartner(string userId);
    }
}
