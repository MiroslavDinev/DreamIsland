namespace DreamIsland.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Islands;
    using DreamIsland.Services.Island;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;
    using Microsoft.AspNetCore.Hosting;
    using System.IO;

    public class IslandsController : ControllerBase
    {
        private readonly IIslandService islandService;
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public IslandsController(IIslandService islandService, IPartnerService partnerService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.islandService = islandService;
            this.partnerService = partnerService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllIslandsQueryModel query)
        {
            var islands = this.islandService.All(query.Region, query.SearchTerm, query.IslandSorting, query.CurrentPage);

            return this.View(islands);
        }

        [Authorize]
        public IActionResult My()
        {
            var userId = this.User.GetUserId();

            var myIslands = this.islandService.GetIslandsByPartner(userId);

            return this.View(myIslands);
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

            return this.View(new IslandAddFormModel
            {
                IslandRegions = this.islandService.GetRegions(),
                PopulationSizes = this.islandService.GetPopulationSizes()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(IslandAddFormModel island)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());
            var controllerName = ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower();

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    controllerName);

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!this.islandService.PopulationSizeExists(island.PopulationSizeId))
            {
                this.ModelState.AddModelError(nameof(island.PopulationSizeId), "Population size does not exist!");
            }

            if (!this.islandService.RegionExists(island.IslandRegionId))
            {
                this.ModelState.AddModelError(nameof(island.IslandRegionId), "Region size does not exist!");
            }

            if (island.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(island.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                }
            }

            if (!ModelState.IsValid)
            {
                island.PopulationSizes = this.islandService.GetPopulationSizes();
                island.IslandRegions = this.islandService.GetRegions();
                return this.View(island);
            }

            string uniqueFileName = await ProcessUploadedFile(island, this.webHostEnvironment, controllerName);

            var islandId = await this.islandService
                .AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price, uniqueFileName, 
                island.PopulationSizeId, island.IslandRegionId, partnerId);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(Details), new { id = islandId, information = island.Name });
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

            var island = this.islandService.Details(id);

            if (island.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var islandForm = this.mapper.Map<IslandEditFormModel>(island);

            islandForm.IslandRegions = this.islandService.GetRegions();
            islandForm.PopulationSizes = this.islandService.GetPopulationSizes();

            return this.View(islandForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(IslandEditFormModel island)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());
            var controllerName = ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower();

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    controllerName);

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!this.islandService.PopulationSizeExists(island.PopulationSizeId))
            {
                this.ModelState.AddModelError(nameof(island.PopulationSizeId), "Population size does not exist!");
            }

            if (!this.islandService.RegionExists(island.IslandRegionId))
            {
                this.ModelState.AddModelError(nameof(island.IslandRegionId), "Region size does not exist!");
            }

            if (!ModelState.IsValid)
            {
                island.PopulationSizes = this.islandService.GetPopulationSizes();
                island.IslandRegions = this.islandService.GetRegions();
                return this.View(island);
            }

            if(!this.islandService.IsByPartner(island.Id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if(island.CoverPhoto != null)
            {
                var isValidImage = IsValidImageFile(island.CoverPhoto);

                if (!isValidImage)
                {
                    ModelState.AddModelError(string.Empty, AllowedImageFormat);
                    return this.View(island);
                }

                if(island.ImageUrl != null)
                {
                    var filePath = Path.Combine(this.webHostEnvironment.WebRootPath, "islands/cover", island.ImageUrl);
                    System.IO.File.Delete(filePath);
                }

                island.ImageUrl = await ProcessUploadedFile(island, this.webHostEnvironment, controllerName);
            }

            var edited = await this.islandService
                .EditAsync(island.Id, island.Name, island.Location, island.Description, island.SizeInSquareKm, 
                island.Price, island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = island.Id, information = island.Name });
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

            if (!this.islandService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var deleted = this.islandService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Details(int id, string information)
        {
            var island = this.islandService.Details(id);

            if(island == null || information != island.Name)
            {
                return NotFound();
            }

            return this.View(island);
        }
    }
}
