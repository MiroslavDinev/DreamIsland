namespace DreamIsland.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Authorization;
    using AutoMapper;

    using DreamIsland.Infrastructure;
    using DreamIsland.Models.Celebrities;
    using DreamIsland.Services.Celebrity;
    using DreamIsland.Services.Partner;

    using static WebConstants.GlobalMessages;
    using System.Linq;
    using System.Collections.Generic;
    using DreamIsland.Models;

    public class CelebritiesController : Controller
    {
        private readonly IPartnerService partnerService;
        private readonly ICelebrityService celebrityService;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CelebritiesController(IPartnerService partnerService, ICelebrityService celebrityService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            this.partnerService = partnerService;
            this.celebrityService = celebrityService;
            this.mapper = mapper;
            this.webHostEnvironment = webHostEnvironment;
        }

        public IActionResult All([FromQuery] AllCelebritiesQueryModel query)
        {
            var celebrities = this.celebrityService.All(query.Occupation, query.SearchTerm, query.CurrentPage);

            return this.View(celebrities);
        }

        [Authorize]
        public IActionResult My()
        {
            var userId = this.User.GetUserId();

            var myCelebrities = this.celebrityService.GetCelebritiesByPartner(userId);

            return this.View(myCelebrities);
        }

        [Authorize]
        public IActionResult Add()
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(CelebrityFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0)
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            if(celebrity.ImageUrl != null)
            {
                string folder = "celebrities/cover/";
                celebrity.ImageUrl = await UploadImage(folder, celebrity.CoverPhoto);
            }

            if (celebrity.GalleryFiles.Any())
            {
                string folder = "celebrities/gallery/";
                celebrity.Gallery = new List<GalleryModel>();

                foreach (var file in celebrity.GalleryFiles)
                {
                    var gallery = new GalleryModel
                    {
                        Name = file.FileName,
                        URL = await UploadImage(folder, file)
                    };

                    celebrity.Gallery.Add(gallery);
                }
            }

            var celebrityId = await this .celebrityService
                .AddAsync(celebrity.Name, celebrity.Occupation, celebrity.Description, celebrity.ImageUrl, celebrity.Age, partnerId, celebrity.Gallery);

            this.TempData[InfoMessageKey] = InfoMessage;

            return RedirectToAction(nameof(Details), new { id = celebrityId, information= celebrity.Name + "-" + celebrity.Occupation });
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var userId = this.User.GetUserId();

            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            var celebrity = this.celebrityService.Details(id);

            if(celebrity.UserId != userId && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var celebrityForm = this.mapper.Map<CelebrityFormModel>(celebrity);

            return this.View(celebrityForm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, CelebrityFormModel celebrity)
        {
            var partnerId = this.partnerService.PartnerId(this.User.GetUserId());

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!ModelState.IsValid)
            {
                return this.View(celebrity);
            }

            if(!this.celebrityService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var edited = await this.celebrityService
                .EditAsync(id, celebrity.Name, celebrity.Occupation, celebrity.Description, 
                celebrity.ImageUrl, celebrity.Age, this.User.IsAdmin());

            if (!edited)
            {
                return BadRequest();
            }

            if (!this.User.IsAdmin())
            {
                this.TempData[InfoMessageKey] = InfoMessage;
            }

            return RedirectToAction(nameof(Details), new { id = id, information = celebrity.Name + "-" + celebrity.Occupation });
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = this.User.GetUserId();
            var partnerId = this.partnerService.PartnerId(userId);

            if (partnerId == 0 && !this.User.IsAdmin())
            {
                this.TempData[WarningMessageKey] = String.Format(WarningMessage, ControllerContext.ActionDescriptor.ActionName.ToLower(),
                    ControllerContext.ActionDescriptor.ControllerName.Replace("Controller", "").ToLower());

                return RedirectToAction(nameof(PartnersController.Become), "Partners");
            }

            if (!this.celebrityService.IsByPartner(id, partnerId) && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var deleted = this.celebrityService.Delete(id);

            if (!deleted)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Details(int id, string information)
        {
            var celebrity = this.celebrityService.Details(id);

            if(!information.Contains(celebrity.Name) && !information.Contains(celebrity.Occupation))
            {
                return BadRequest();
            }

            return this.View(celebrity);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(this.webHostEnvironment.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
    }
}
