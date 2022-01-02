namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Islands;
    using DreamIsland.Services.Island;
    using DreamIsland.Services.Partner;

    public class IslandsController : Controller
    {
        private readonly IIslandService islandService;
        private readonly IPartnerService partnerService;

        public IslandsController(IIslandService islandService, IPartnerService partnerService)
        {
            this.islandService = islandService;
            this.partnerService = partnerService;
        }

        public IActionResult All([FromQuery] AllIslandsQueryModel query)
        {
            var islands = this.islandService.All(query.Region, query.SearchTerm, query.IslandSorting, query.CurrentPage);

            return this.View(islands);
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

            return this.View(new AddFormIslandModel
            {
                IslandRegions = this.islandService.GetRegions(),
                PopulationSizes = this.islandService.GetPopulationSizes()
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddFormIslandModel island)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if(partnerId == 0)
            {
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

            await this.islandService
                .AddAsync(island.Name, island.Location, island.Description, island.SizeInSquareKm, island.Price, island.ImageUrl, 
                island.PopulationSizeId, island.IslandRegionId, partnerId);

            return RedirectToAction("Index", "Home");
        }
    }
}
