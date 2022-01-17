namespace DreamIsland.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authorization;
    using AutoMapper;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;


    public class CelebritiesController : ControllerBase
    {
        private readonly IPartnerService partnerService;
        private readonly ICelebrityService celebrityService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CelebritiesController(IPartnerService partnerService, ICelebrityService celebrityService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.partnerService = partnerService;
            this.celebrityService = celebrityService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
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

            if(celebrity.ImageUrl == null)
            {
                string folder = "celebrities/cover/";
                celebrity.ImageUrl = await UploadImage(folder, celebrity.CoverPhoto, this.webHostEnvironment);
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            var celebrityId = await this .celebrityService
                .AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, celebrity.ImageUrl, celebrity.Age, partnerId);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(Details), new { id = celebrityId, information= celebrity.Name + "-" + celebrity.Occupation });
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

            if (celebrity.ImageUrl == null)
            {
                string folder = "celebrities/cover/";
                celebrity.ImageUrl = await UploadImage(folder, celebrity.CoverPhoto, this.webHostEnvironment);
            }

            var edited = await this.celebrityService
                .EditAsync(id, celebrity.Name, celebrity.Occupation, celebrity.Description, 
                celebrity.ImageUrl, celebrity.Age, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = id, information = celebrity.Name + "-" + celebrity.Occupation });
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

            if (!this.celebrityService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var deleted = this.celebrityService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id, string information)
        {
            var celebrity = this.celebrityService.Details(id);

            if(!information.Contains(celebrity.Name) && !information.Contains(celebrity.Occupation))
            {
                return BadRequest();
            }

            return this.View(celebrity);
        }
    }
}
