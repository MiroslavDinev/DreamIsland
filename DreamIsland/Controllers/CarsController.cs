namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Models.Cars;
    using DreamIsland.Services.Car;
    using DreamIsland.Services.Partner;
    using DreamIsland.Infrastructure;
    using AutoMapper;

    public class CarsController : Controller
    {
        private readonly ICarService carService;
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;

        public CarsController(ICarService carService, IPartnerService partnerService, IMapper mapper)
        {
            this.carService = carService;
            this.partnerService = partnerService;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllCarsQueryModel query)
        {
            var cars = this.carService
                .All(query.Brand, query.SearchTerm, query.CarsSorting, query.CurrentPage);

            return this.View(cars);
        }

        [Authorize]
        public IActionResult My()
        {
            var userId = this.User.GetUserId();

            var myCars = this.carService.GetCarsByPartner(userId);

            return this.View(myCars);
        }

        [Authorize]
        public IActionResult Add()
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CarFormModel car)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
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
                car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager, partnerId);

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetUserId();

            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0)
            {
                // visualize message to be partner before editing

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            var car = this.carService.Details(id);

            if(car.UserId != userId)
            {
                return Unauthorized();
            }

            var carForm = this.mapper.Map<CarFormModel>(car);

            return this.View(carForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CarFormModel car)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                // visualize message to be partner before editing

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(car);
            }

            if (!this.carService.IsByPartner(id, partnerId))
            {
                return Unauthorized();
            }

            var edited = await this.carService
                .EditAsync(id, car.Brand, car.Model, car.Description, car.ImageUrl, 
                car.Year, car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
