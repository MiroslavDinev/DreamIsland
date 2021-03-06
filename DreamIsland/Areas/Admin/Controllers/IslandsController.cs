namespace DreamIsland.Areas.Admin.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Services.Island;
    using DreamIsland.Areas.Admin.Models.Island;

    public class IslandsController : AdminController
    {
        private readonly IIslandService islandService;

        public IslandsController(IIslandService islandService)
        {
            this.islandService = islandService;
        }
        public IActionResult All([FromQuery] AllAdminIslandsQueryModel query)
        {
            var island = this.islandService.AllAdmin(query.CurrentPage);

            return this.View(island);
        }

        public async Task<IActionResult> ChangeStatus(int id)
        {
            var changed = await this.islandService.ChangeStatus(id);

            if (!changed)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
