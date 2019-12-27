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
using Microsoft.AspNetCore.Http.Features;

namespace CoreApp1.Controllers
{
    [Authorize]
    public class FileUploadController : Controller
    {
        private readonly Services services;
        public FileUploadController()
        {
            services = new Services();
        }
        public IActionResult Index()
        {
            ViewBag.userIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return View(Startup.fileUploadModels);
        }
        [HttpPost("FileUpload")]
        public IActionResult Index(List<IFormFile> files)
        {
            string name = ControllerContext.HttpContext.User.Identity.Name;

            services.UploadFiles(files, name);

            return RedirectToAction("Index");
        }
        //Download Action
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.StoragePath, filename);

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
        //Delete Action
        public IActionResult Delete(string filename)
        {
            services.DeleteFile(filename);

            return RedirectToAction("Index");
        }
    }
}