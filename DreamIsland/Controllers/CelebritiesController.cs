namespace DreamIsland.Controllers
{
    using System;
    using System.IO;
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

        public CelebritiesController(ICelebrityService celebrityService, IPartnerService partnerService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.celebrityService = celebrityService;
            this.partnerService = partnerService;
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
        public async Task<IActionResult> Add(CelebrityAddFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());
            var controllerName = ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower();

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    controllerName);

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (celebrity.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(celebrity.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                }
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            string uniqueFileName = await ProcessUploadedFile(celebrity, this.webHostEnvironment, controllerName);

            var celebrityId = await this .celebrityService
                .AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, uniqueFileName, celebrity.Age, partnerId);

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

            var celebrityForm = this.mapper.Map<CelebrityEditFormModel>(celebrity);

            return this.View(celebrityForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CelebrityEditFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());
            var controllerName = ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower();

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    controllerName);

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            if (!this.celebrityService.IsByPartner(celebrity.Id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }           

            if(celebrity.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(celebrity.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                    return this.View(celebrity);
                }

                if (celebrity.ImageUrl != null)
                {
                    var filePath = Path.Combine(this.webHostEnvironment.WebRootPath, "celebrities/cover", celebrity.ImageUrl);
                    System.IO.File.Delete(filePath);
                }

                celebrity.ImageUrl = await ProcessUploadedFile(celebrity, this.webHostEnvironment, controllerName);                
            }

            var edited = await this.celebrityService
                .EditAsync(celebrity.Id, celebrity.Name, celebrity.Occupation, celebrity.Description,
                celebrity.ImageUrl, celebrity.Age, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = celebrity.Id, information = celebrity.Name + "-" + celebrity.Occupation });
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
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

            var deleted = await this.celebrityService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var celebrity = this.celebrityService.Details(id);

            if(celebrity==null || (!information.Contains(celebrity.Name) && !information.Contains(celebrity.Occupation)))
            {
                return NotFound();
            }

            return this.View(celebrity);
        }
    }
}
