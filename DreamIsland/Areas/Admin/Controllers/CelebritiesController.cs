namespace DreamIsland.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using DreamIsland.Services.Celebrity;
    using DreamIsland.Areas.Admin.Models.Celebrity;

    public class CelebritiesController : AdminController
    {
        private readonly ICelebrityService celebrityService;

        public CelebritiesController(ICelebrityService celebrityService)
        {
            this.celebrityService = celebrityService;
        }

        public IActionResult All([FromQuery] AllAdminCelebritiesQueryModel query)
        {
            var celebrity = this.celebrityService.AllAdmin(query.CurrentPage);

            return this.View(celebrity);
        }

        public IActionResult ChangeStatus(int id)
        {
            var changed = this.celebrityService.ChangeStatus(id);

            if (!changed)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(All));
        }
    }
}
