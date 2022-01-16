namespace DreamIsland.Services.Celebrity
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DreamIsland.Areas.Admin.Models.Celebrity;
    using DreamIsland.Models;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity.Models;

    public interface ICelebrityService
    {
        Task<int> AddAsync(string name, string occupation, string description, string imageUrl, int? age, int partnerId);

        Task<bool> EditAsync(int celebrityId, string name, string occupation, string description, string imageUrl, int? age, bool isPublic);

        AllCelebritiesQueryModel All(string occupation = null, string searchTerm = null, int currentPage = 1);

        AllAdminCelebritiesQueryModel AllAdmin(int currentPage = 1);

        bool ChangeStatus(int celebrityId);
        bool Delete(int celebrityId);

        IEnumerable<CelebrityListingViewModel> GetCelebritiesByPartner(string userId);

        CelebrityDetailsServiceModel Details(int celebrityId);

        bool IsByPartner(int celebrityId, int partnerId);
    }
}
