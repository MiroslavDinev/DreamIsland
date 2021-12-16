namespace DreamIsland.Controllers
{
    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Partners;
    using DreamIsland.Services.Partner;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

            return RedirectToAction("Index", "Home");
        }
    }
}
