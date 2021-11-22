using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.IServices;

namespace WebApplication1.Services
{
    public class CsvService : ICsvService
    {
        public string[] GetCsv(IFormFile postedFile, IHostingEnvironment environment)
        {
            string path = Path.Combine(environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(postedFile.FileName);
            string filePath = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }

            return System.IO.File.ReadAllLines(filePath);
        }
    }
}
