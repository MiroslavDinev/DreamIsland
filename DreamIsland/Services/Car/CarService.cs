namespace DreamIsland.Services.Car
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using DreamIsland.Data;
    using Data.Models.Vehicles;
    using DreamIsland.Models.Cars;
    using DreamIsland.Models.Cars.Enums;

    public class CarService : ICarService
    {
        private readonly DreamIslandDbContext data;
        private readonly IMapper mapper;

        public CarService(DreamIslandDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
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

        public AllCarsQueryModel All(string brand = null, string searchTerm = null, CarsSorting carSorting = CarsSorting.DateAdded, int currentPage = 1)
        {
            var carsQuery = this.data
                .Cars
                .AsQueryable();

            if (!string.IsNullOrEmpty(brand))
            {
                carsQuery = carsQuery
                    .Where(x => x.Brand == brand);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                carsQuery = carsQuery
                    .Where(x => x.Model.ToLower().Contains(searchTerm.ToLower()));
            }

            carsQuery = carSorting switch 
            {
                CarsSorting.YearAscending => carsQuery.OrderBy(x=> x.Year),
                CarsSorting.YearDescending => carsQuery.OrderByDescending(x=> x.Year),
                CarsSorting.DateAdded or _ => carsQuery.OrderByDescending(x=> x.Id)
            };

            var totalCars = carsQuery.Count();

            var cars = this.GetCars(carsQuery
                .Skip((currentPage - 1) * AllCarsQueryModel.ItemsPerPage)
                .Take(AllCarsQueryModel.ItemsPerPage));
                

            var brands = this.data.Cars
                .Select(x => x.Brand)
                .Distinct()
                .OrderBy(x=>x)
                .ToList();

            var car = new AllCarsQueryModel
            {
                Cars = cars,
                Brands = brands,
                CurrentPage = currentPage,
                TotalItems = totalCars,
                Brand = brand,
                CarsSorting = carSorting,
                SearchTerm = searchTerm
            };

            return car;
        }

        public IEnumerable<CarListingViewModel> GetCarsByPartner(string userId)
        {
            var cars = this.GetCars(this.data
                .Cars
                .Where(x => x.Partner.UserId == userId)
                .OrderByDescending(x => x.Id));

            return cars;
        }
        private IEnumerable<CarListingViewModel> GetCars(IQueryable carsQuery)
        {
            var cars = carsQuery
                .ProjectTo<CarListingViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return cars;
        }
    }
}
