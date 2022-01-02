namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Partner;

    public class CelebritiesController : Controller
    {
        private readonly IPartnerService partnerService;
        private readonly ICelebrityService celebrityService;

        public CelebritiesController(IPartnerService partnerService, ICelebrityService celebrityService)
        {
            this.partnerService = partnerService;
            this.celebrityService = celebrityService;
        }

        public IActionResult All([FromQuery] AllCelebritiesQueryModel query)
        {
            var celebrities = this.celebrityService.All(query.Occupation, query.SearchTerm, query.CurrentPage);

            return this.View(celebrities);
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
        public async Task<IActionResult> Add(AddCelebrityFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                // visualize message to be partner before adding

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            await this .celebrityService.AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, celebrity.ImageUrl, celebrity.Age, partnerId);

            return RedirectToAction(nameof(All));
        }
    }
}
