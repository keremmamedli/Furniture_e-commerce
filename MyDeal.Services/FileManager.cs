using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDeal.Services
{
    public static class FileManager
    {
        public static bool CheckFile(this IFormFile file, int length)
        {
            return file.ContentType.Contains("image/") && file.Length / 1024 / 1024 < length;
        }

        public static async Task<string> UploadFile(this IFormFile file, string web, string path)
        {
            string FolderPath = Path.Combine(web, path);

            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }

            string fileName = Guid.NewGuid().ToString() + file.FileName;

            string UploadPath = Path.Combine(FolderPath, fileName);

            using (var stream = new FileStream(UploadPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
