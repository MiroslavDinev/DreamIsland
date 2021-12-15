namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Models.Cars;
    using DreamIsland.Services.Car;

    public class CarsController : Controller
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        public IActionResult All()
        {
            var cars = this.carService.All();

            return this.View(cars);
        }
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarFormModel car)
        {
            if (!ModelState.IsValid)
            {
                return this.View(car);
            }

            await this.carService.AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, 
                car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager);

            return RedirectToAction(nameof(All));
        }
    }
}
