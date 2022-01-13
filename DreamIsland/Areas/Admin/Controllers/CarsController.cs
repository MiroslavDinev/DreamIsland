namespace DreamIsland.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Services.Car;
    using DreamIsland.Areas.Admin.Models.Car;

    public class CarsController : AdminController
    {
        private readonly ICarService carService;

        public CarsController(ICarService carService)
        {
            this.carService = carService;
        }

        public IActionResult All([FromQuery] AllAdminCarsQueryModel query)
        {
            var car = this.carService.AllAdmin(query.CurrentPage);

            return this.View(car);
        }

        public IActionResult ChangeStatus(int id)
        {
            var changed = this.carService.ChangeStatus(id);

            if (!changed)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
