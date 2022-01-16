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

    public class IslandsController : ControllerBase
    {
        private readonly IIslandService islandService;
        private readonly IPartnerService partnerService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment hostEnvironment;

        public IslandsController(IIslandService islandService, IPartnerService partnerService, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            this.islandService = islandService;
            this.partnerService = partnerService;
            this.mapper = mapper;
            this.hostEnvironment = hostEnvironment;
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

            return this.View(new IslandFormModel
            {
                IslandRegions = this.islandService.GetRegions(),
                PopulationSizes = this.islandService.GetPopulationSizes()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(IslandFormModel island)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if(partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

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

            if (island.ImageUrl == null)
            {
                string folder = "islands/cover/";
                island.ImageUrl = await UploadImage(folder, island.CoverPhoto, this.hostEnvironment);
            }

            if (!ModelState.IsValid)
            {
                island.PopulationSizes = this.islandService.GetPopulationSizes();
                island.IslandRegions = this.islandService.GetRegions();
                return this.View(island);
            }

            var islandId = await this.islandService
                .AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price, island.ImageUrl, 
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

            var islandForm = this.mapper.Map<IslandFormModel>(island);

            islandForm.IslandRegions = this.islandService.GetRegions();
            islandForm.PopulationSizes = this.islandService.GetPopulationSizes();

            return this.View(islandForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, IslandFormModel island)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

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

            if(!this.islandService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var edited = await this.islandService
                .EditAsync(id, island.Name, island.Location, island.Description, island.SizeInSquareKm, 
                island.Price, island.ImageUrl, island.PopulationSizeId, island.IslandRegionId, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = id, information = island.Name });
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

        public IActionResult Details(int id, string information)
        {
            var island = this.islandService.Details(id);

            if(information != island.Name)
            {
                return BadRequest();
            }

            return this.View(island);
        }
    }
}
