namespace DreamIsland.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using AutoMapper;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;

    public class CelebritiesController : Controller
    {
        private readonly IPartnerService partnerService;
        private readonly ICelebrityService celebrityService;
        private readonly IMapper mapper;

        public CelebritiesController(IPartnerService partnerService, ICelebrityService celebrityService, IMapper mapper)
        {
            this.partnerService = partnerService;
            this.celebrityService = celebrityService;
            this.mapper = mapper;
        }

        public IActionResult All([FromQuery] AllCelebritiesQueryModel query)
        {
            var celebrities = this.celebrityService.All(query.Occupation, query.SearchTerm, query.CurrentPage);

            return this.View(celebrities);
        }

        [Authorize]
        public IActionResult My()
        {
            var userId = this.User.GetUserId();

            var myCelebrities = this.celebrityService.GetCelebritiesByPartner(userId);

            return this.View(myCelebrities);
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
        public async Task<IActionResult> Add(CelebrityFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            await this .celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, celebrity.ImageUrl, celebrity.Age, partnerId);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(All));
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

            var celebrity = this.celebrityService.Details(id);

            if(celebrity.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var celebrityForm = this.mapper.Map<CelebrityFormModel>(celebrity);

            return this.View(celebrityForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CelebrityFormModel celebrity)
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
                return this.View(celebrity);
            }

            if(!this.celebrityService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var edited = await this.celebrityService
                .EditAsync(id, celebrity.Name, celebrity.Occupation, celebrity.Description, 
                celebrity.ImageUrl, celebrity.Age);

            if (!edited)
            {
                return BadRequest();
            }

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(All));
        }
    }
}
