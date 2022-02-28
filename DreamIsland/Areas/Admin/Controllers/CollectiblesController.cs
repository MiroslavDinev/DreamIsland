namespace DreamIsland.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Services.Collectible;
    using DreamIsland.Areas.Admin.Models.Collectible;

    public class CollectiblesController : AdminController
    {
        private readonly ICollectibleService collectibleService;

        public CollectiblesController(ICollectibleService collectibleService)
        {
            this.collectibleService = collectibleService;
        }

        public IActionResult All([FromQuery] AllAdminCollectiblesQueryModel query)
        {
            var collectible = this.collectibleService.AllAdmin(query.CurrentPage);

            return this.View(collectible);
        }

        public IActionResult ChangeStatus(int id)
        {
            var changed = this.collectibleService.ChangeStatus(id);

            if (!changed)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
