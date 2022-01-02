namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Services.Partner;

    public class CollectiblesController : Controller
    {
        private readonly IPartnerService partnerService;
        private readonly ICollectibleService collectibleService;

        public CollectiblesController(IPartnerService partnerService, ICollectibleService collectibleService)
        {
            this.partnerService = partnerService;
            this.collectibleService = collectibleService;
        }

        public IActionResult All([FromQuery] AllCollectiblesQueryModel query)
        {
            var collectibles = this.collectibleService.All(query.RarityLevel, query.SearchTerm, query.CurrentPage);

            return this.View(collectibles);
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
        public async Task<IActionResult> Add(AddCollectibleFormModel collectible)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(collectible);
            }

            await this.collectibleService.AddAsync(collectible.Name, collectible.Description, collectible.ImageUrl, collectible.RarityLevel, partnerId);

            return RedirectToAction(nameof(All));
        }
    }
}
