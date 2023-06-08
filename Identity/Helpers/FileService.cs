using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Identity.Helpers
{
    public static class FileService
    {
        private static readonly IWebHostEnvironment _webHostEnvironment;

        public static string UploadedFile(IFormFile logo)
        {
            if (logo == null || logo.Length <= 0)
                return null;

            string uniqueFileName = $"{Guid.NewGuid()}_{logo.FileName}";
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "OrganizationSetups");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                logo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }
    }
}
