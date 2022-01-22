namespace DreamIsland.Controllers
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Hosting;

    using DreamIsland.Models.Contracts;

    public class ControllerBase : Controller
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };
        public async Task<string> ProcessUploadedFile(IFormModel formModel, IWebHostEnvironment webHostEnvironment, string folderName)
        {
            string uniqueFileName = null;
            if (formModel.CoverPhoto != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, $"{folderName}/cover");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + formModel.CoverPhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formModel.CoverPhoto.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }

        public bool IsValidImageFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).TrimStart('.');

            if (!this.allowedExtensions.Any(x => extension.EndsWith(x)))
            {
                return false;
            }

            return true;
        }
    }
}
