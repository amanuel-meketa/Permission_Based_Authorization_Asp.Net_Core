using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Identity.Helpers
{
    public static class FileService
    {
        public static string UploadedFile(IFormFile logo, IWebHostEnvironment webHostEnvironment)
        {
            if (logo == null || logo.Length <= 0)
                return null;

            string uniqueFileName = $"{Guid.NewGuid()}_{logo.FileName}";
            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", "OrganizationSetups");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            Directory.CreateDirectory(uploadsFolder);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                logo.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public static bool DeleteFile(string fileName, IWebHostEnvironment webHostEnvironment)
        {
            if (string.IsNullOrEmpty(fileName))
                return false;

            string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", "OrganizationSetups");
            string filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);

            return true;
        }
    }
}

