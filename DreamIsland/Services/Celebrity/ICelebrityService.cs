namespace DreamIsland.Services.Celebrity
{
    using System.Threading.Tasks;

    using DreamIsland.Models.Celebrities;

    public interface ICelebrityService
    {
        Task<int> AddAsync(string name, string occupation, string description, string imageUrl, int? age, int partnerId);

        AllCelebritiesQueryModel All(string occupation = null, string searchTerm = null);
    }
}
