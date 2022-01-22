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
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;

    public class CollectiblesController : ControllerBase
    {
        private readonly IPartnerService partnerService;
        private readonly ICollectibleService collectibleService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CollectiblesController(IPartnerService partnerService, ICollectibleService collectibleService, 
            IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.partnerService = partnerService;
            this.collectibleService = collectibleService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllCollectiblesQueryModel query)
        {
            var collectibles = this.collectibleService.All(query.RarityLevel, query.SearchTerm, query.CurrentPage);

            return this.View(collectibles);
        }

        [Authorize]
        public IActionResult My()
        {
            var userId = this.User.GetUserId();

            var myCollectibles = this.collectibleService.GetCollectiblesByPartner(userId);

            return this.View(myCollectibles);
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
        public async Task<IActionResult> Add(CollectibleAddFormModel collectible)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());
            var controllerName = ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower();

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    controllerName);

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if(collectible.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(collectible.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                }
            }

            if (!ModelState.IsValid)
            {
                return this.View(collectible);
            }

            string uniqueFileName = await ProcessUploadedFile(collectible, this.webHostEnvironment, controllerName);

            var collectibleId = await this.collectibleService.AddAsync(collectible.Name, collectible.Description, uniqueFileName, 
                collectible.RarityLevel, partnerId);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(Details), new { id = collectibleId, information = collectible.Name });
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

            var collectible = this.collectibleService.Details(id);

            if(collectible.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var collectibleForm = this.mapper.Map<CollectibleEditFormModel>(collectible);

            return this.View(collectibleForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(CollectibleEditFormModel collectible)
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
                return this.View(collectible);
            }

            if(!this.collectibleService.IsByPartner(collectible.Id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (collectible.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(collectible.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                    return this.View(collectible);
                }
                
                if(collectible.ImageUrl != null)
                {
                    var filePath = Path.Combine(this.webHostEnvironment.WebRootPath, "collectibles/cover", collectible.ImageUrl);
                    System.IO.File.Delete(filePath);
                }

                collectible.ImageUrl = await ProcessUploadedFile(collectible, this.webHostEnvironment, controllerName);
            }

            var edited = await this.collectibleService
                .EditAsync(collectible.Id, collectible.Name, collectible.Description, collectible.ImageUrl,
                collectible.RarityLevel, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }           

            return RedirectToAction(nameof(Details), new { id = collectible.Id, information = collectible.Name });
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

            if (!this.collectibleService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var deleted = this.collectibleService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id, string information)
        {
            var collectible = this.collectibleService.Details(id);

            if (!information.Contains(collectible.Name))
            {
                return BadRequest();
            }

            return this.View(collectible);
        }
    }
}
