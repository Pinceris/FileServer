using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HeyRed.Mime;
using Microsoft.AspNetCore.Authorization;

namespace CoreApp1.Controllers
{
    [Authorize]
    public class FileUploadController : Controller
    {
        //folder where uploads are kept
        private readonly string StoragePath = "FileStorage";

        public IActionResult Index()
        {

            return View(Startup.fileUploadModels);
        }
        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.Join(StoragePath, formFile.FileName);
                    //Save File Info
                    FileUploadModel fileUploadModel = new FileUploadModel()
                    {
                        Id = Guid.NewGuid(),
                        Author = ControllerContext.HttpContext.User.Identity.Name,
                        FileName = formFile.FileName,
                        Created_At = DateTime.Now,
                        Downloads = 0
                    };
                    Startup.fileUploadModels.Add(fileUploadModel);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return RedirectToAction("Index");
        }
        //Download Action
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), StoragePath, filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            //update download counter
            Startup.fileUploadModels.FirstOrDefault(q => q.FileName == filename).Downloads++;

            memory.Position = 0;
            return File(memory, MimeTypesMap.GetMimeType(filename), Path.GetFileName(path));
        }
    }
}