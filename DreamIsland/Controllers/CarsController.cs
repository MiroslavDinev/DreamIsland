namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Models.Cars;
    using DreamIsland.Services.Car;
    using DreamIsland.Services.Partner;
    using DreamIsland.Infrastructure;

    public class CarsController : Controller
    {
        private readonly ICarService carService;
        private readonly IPartnerService partnerService;

        public CarsController(ICarService carService, IPartnerService partnerService)
        {
            this.carService = carService;
            this.partnerService = partnerService;
        }

        public IActionResult All()
        {
            var cars = this.carService
                .All();

            return this.View(cars);
        }

        [Authorize]
        public IActionResult Add()
        {            
            if (this.GetPartnerId() == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCarFormModel car)
        {
            if (this.GetPartnerId() == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(car);
            }

            await this.carService
                .AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, 
                car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager);

            return RedirectToAction(nameof(All));
        }

        private int GetPartnerId()
        {
            var userId = this.User.GetUserId();

            var partnerId = this.partnerService.PartnerId(userId);

            return partnerId;
        }
    }
}
