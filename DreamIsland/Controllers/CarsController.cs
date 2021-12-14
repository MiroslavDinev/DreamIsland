namespace DreamIsland.Controllers
{
    using DreamIsland.Models.Cars;
    using DreamIsland.Services.Car;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CarsController : Controller
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
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

            return RedirectToAction("Index", "Home");
        }
    }
}
