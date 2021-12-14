namespace DreamIsland.Services.Car
{
    using System.Threading.Tasks;

    public interface ICarService
    {
        Task<int> AddAsync(string brand, string model, string description, string imageUrl, int year,
            bool hasRemoteStart, bool hasRemoteControlParking, bool hasSeatMassager);
    }
}
