namespace DreamIsland.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class IslandsController : AdminController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
