namespace DreamIsland.Services.Car
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DreamIsland.Data;
    using Data.Models.Vehicles;
    using DreamIsland.Models.Cars;

    public class CarService : ICarService
    {
        private readonly DreamIslandDbContext data;

        public CarService(DreamIslandDbContext data)
        {
            this.data = data;
        }
        public async Task<int> AddAsync(string brand, string model, string description, string imageUrl, int year, bool hasRemoteStart, bool hasRemoteControlParking, bool hasSeatMassager, int partnerId)
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
                HasSeatMassager = hasSeatMassager,
                PartnerId = partnerId
            };

            await this.data.Cars.AddAsync(car);
            await this.data.SaveChangesAsync();

            return car.Id;
        }

        public IEnumerable<CarListingViewModel> All()
        {
            var cars = this.data.Cars
                .OrderByDescending(c=> c.Id)
                .Select(x => new CarListingViewModel
                {
                    Id = x.Id,
                    Brand = x.Brand,
                    Model = x.Model,
                    ImageUrl = x.ImageUrl,
                    Year = x.Year
                })
                .ToList();

            return cars;
        }
    }
}
