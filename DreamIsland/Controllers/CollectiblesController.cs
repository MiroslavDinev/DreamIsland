namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Collectibles;
    using DreamIsland.Services.Collectible;
    using DreamIsland.Services.Partner;
    using AutoMapper;

    public class CollectiblesController : Controller
    {
        private readonly IPartnerService partnerService;
        private readonly ICollectibleService collectibleService;
        private readonly IMapper mapper;

        public CollectiblesController(IPartnerService partnerService, ICollectibleService collectibleService, IMapper mapper)
        {
            this.partnerService = partnerService;
            this.collectibleService = collectibleService;
            this.mapper = mapper;
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
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CollectibleFormModel collectible)
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

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetUserId();

            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            var collectible = this.collectibleService.Details(id);

            if(collectible.UserId != userId)
            {
                return Unauthorized();
            }

            var collectibleForm = this.mapper.Map<CollectibleFormModel>(collectible);

            return this.View(collectibleForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CollectibleFormModel collectible)
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

            if(!this.collectibleService.IsByPartner(id, partnerId))
            {
                return Unauthorized();
            }

            var edited = await this.collectibleService
                .EditAsync(id, collectible.Name, collectible.Description, collectible.ImageUrl, collectible.RarityLevel);

            if (!edited)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
