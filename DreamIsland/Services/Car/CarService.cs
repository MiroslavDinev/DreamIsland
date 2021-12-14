namespace DreamIsland.Services.Car
{
    using System.Threading.Tasks;

    using DreamIsland.Data;
    using Data.Models.Vehicles;

    public class CarService : ICarService
    {
        private readonly DreamIslandDbContext data;

        public CarService(DreamIslandDbContext data)
        {
            this.data = data;
        }
        public async Task<int> AddAsync(string brand, string model, string description, string imageUrl, int year, bool hasRemoteStart, bool hasRemoteControlParking, bool hasSeatMassager)
        {
            var car = new Car
            {
                Brand = brand,
                Model = model,
                Description = description,
                ImageUrl = imageUrl,
                Year = year,
                HasRemoteStart = hasRemoteStart,
                HasRemoteControlParking = hasRemoteControlParking,
                HasSeatMassager = hasSeatMassager
            };

            await this.data.Cars.AddAsync(car);
            await this.data.SaveChangesAsync();

            return car.Id;
        }
    }
}
