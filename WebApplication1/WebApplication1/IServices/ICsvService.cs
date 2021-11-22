using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.IServices
{
    public interface ICsvService
    {
        string[] GetCsv(IFormFile postedFile, IHostingEnvironment environment);
    }
}
