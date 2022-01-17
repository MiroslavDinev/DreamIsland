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
    using DreamIsland.Services.Car.Models;
    using DreamIsland.Areas.Admin.Models.Car;

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
                PartnerId = partnerId,
                IsPublic = false
            };

            await this.data.Cars.AddAsync(car);
            await this.data.SaveChangesAsync();

            return car.Id;
        }

        public AllCarsQueryModel All(string brand = null, string searchTerm = null, CarsSorting carSorting = CarsSorting.DateAdded, int currentPage = 1)
        {
            var carsQuery = this.data
                .Cars
                .Where(c=> c.IsPublic && !c.IsDeleted);

            if (!string.IsNullOrEmpty(brand))
            {
                carsQuery = carsQuery
                    .Where(x => x.Brand == brand);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                carsQuery = carsQuery
                    .Where(x => (x.Brand + " " + x.Model).ToLower().Contains(searchTerm.ToLower()));
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

        public AllAdminCarsQueryModel AllAdmin(int currentPage = 1)
        {
            var carsQuery = this.data
                .Cars
                .Where(x => !x.IsDeleted);

            var totalCars = carsQuery.Count();

            var cars = this.GetCars(carsQuery
                .OrderBy(x=> x.Id)
                .Skip((currentPage - 1) * AllAdminCarsQueryModel.ItemsPerPage)
                .Take(AllAdminCarsQueryModel.ItemsPerPage));

            var car = new AllAdminCarsQueryModel
            {
                Cars = cars,
                CurrentPage = currentPage,
                TotalItems = totalCars
            };

            return car;
        }

        public bool ChangeStatus(int carId)
        {
            var car = this.data.Cars.Find(carId);

            if(car == null)
            {
                return false;
            }
            else if (car.IsDeleted)
            {
                return false;
            }

            car.IsPublic = !car.IsPublic;

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int carId)
        {
            var car = this.data.Cars.Find(carId);

            if(car == null)
            {
                return false;
            }
            else if (car.IsDeleted)
            {
                return false;
            }

            car.IsDeleted = true;

            this.data.SaveChanges();

            return true;
        }

        public CarDetailsServiceModel Details(int carId)
        {
            var car = this.data
                .Cars
                .Where(x => x.Id == carId && !x.IsDeleted)
                .ProjectTo<CarDetailsServiceModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefault();

            return car;
        }

        public async Task<bool> EditAsync(int carId, string brand, string model, string description,
            string imageUrl, int year, bool hasRemoteStart, bool hasRemoteControlParking,
            bool hasSeatMassager, bool isPublic)
        {
            var car = this.data.Cars.Find(carId);

            if(car == null)
            {
                return false;
            }
            else if (car.IsDeleted)
            {
                return false;
            }

            car.Brand = brand;
            car.Model = model;
            car.Description = description;
            car.ImageUrl = imageUrl;
            car.Year = year;
            car.HasRemoteStart = hasRemoteStart;
            car.HasRemoteControlParking = hasRemoteControlParking;
            car.HasSeatMassager = hasSeatMassager;
            car.IsPublic = isPublic;

            await this.data.SaveChangesAsync();

            return true;
        }

        public IEnumerable<CarListingViewModel> GetCarsByPartner(string userId)
        {
            var cars = this.GetCars(this.data
                .Cars
                .Where(x => x.Partner.UserId == userId && !x.IsDeleted)
                .OrderByDescending(x => x.Id));

            return cars;
        }

        public bool IsByPartner(int carId, int partnerId)
        {
            var isByPartner = this.data
                .Cars
                .Any(x => x.Id == carId && x.PartnerId == partnerId);

            return isByPartner;
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
