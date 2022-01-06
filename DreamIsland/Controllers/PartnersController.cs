namespace DreamIsland.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Partners;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;

    public class PartnersController : Controller
    {
        private readonly IPartnerService partnerService;

        public PartnersController(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        [Authorize]
        public IActionResult Become()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Become(BecomePartnerFormModel partner)
        {
            var userId = this.User.GetUserId();

            var userIsPartner = this.partnerService
                .isPartner(userId);

            if (userIsPartner)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.View(partner);
            }

            await this.partnerService.BecomePartner(partner.Name, partner.PhoneNumber, userId);

            this.TempData[SuccessMessageKey] = SuccessMessagePartner;

            return RedirectToAction("Index", "Home");
        }
    }
}
