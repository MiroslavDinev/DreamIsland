namespace DreamIsland.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using AutoMapper;

    using DreamIsland.Models.Cars;
    using DreamIsland.Services.Car;
    using DreamIsland.Services.Partner;
    using DreamIsland.Infrastructure;

    using static WebConstants.GlobalMessages;

    public class CarsController : ControllerBase
    {
        private readonly ICarService carService;
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CarsController(ICarService carService, IPartnerService partnerService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.carService = carService;
            this.partnerService = partnerService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
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
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

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
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if(car.ImageUrl == null)
            {
                string folder = "cars/cover/";
                car.ImageUrl = await UploadImage(folder, car.CoverPhoto, this.webHostEnvironment);
            }

            if (!ModelState.IsValid)
            {
                return this.View(car);
            }

            var carId = await this.carService
                .AddAsync(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, 
                car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager, partnerId);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(Details), new { id=carId, information= car.Brand + "-" + car.Model });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetUserId();

            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            var car = this.carService.Details(id);

            if(car.UserId != userId && !this.User.IsAdmin())
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

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(car);
            }

            if (!this.carService.IsByPartner(id, partnerId)  && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var edited = await this.carService
                .EditAsync(id, car.Brand, car.Model, car.Description, car.ImageUrl, 
                car.Year, car.HasRemoteStart, car.HasRemoteControlParking, car.HasSeatMassager, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = id, information = car.Brand + "-" + car.Model });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.GetUserId();
            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!this.carService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var deleted = this.carService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));

        }

        public IActionResult Details(int id, string information)
        {
            var car = this.carService.Details(id);

            if(!information.Contains(car.Model) && !information.Contains(car.Brand))
            {
                return BadRequest();
            }

            return this.View(car);
        }
    }
}
