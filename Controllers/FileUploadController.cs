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
using CoreApp1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CoreApp1.Controllers
{
    [Authorize]
    public class FileUploadController : Controller
    {
        private readonly UserManager<CoreApp1User> _userManager;
        private readonly SignInManager<CoreApp1User> _signInManager;
        private readonly Services _services;
        public FileUploadController(
            UserManager<CoreApp1User> userManager,
            SignInManager<CoreApp1User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _services = new Services();
        }

        public IActionResult Index()
        {
            ViewBag.userIP = Request.HttpContext.Connection.LocalIpAddress.ToString();

            return View(Startup.fileUploadModels);
        }
        [HttpPost("FileUpload")]
        public async Task<IActionResult> IndexAsync(List<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);

            if(!user.EmailConfirmed)
            {
                TempData["alert"] = Services.Constants.AlertString;
                return RedirectToAction("Index");
            }

            string name = ControllerContext.HttpContext.User.Identity.Name;

            _services.UploadFiles(files, name);

            return RedirectToAction("Index");
        }
        //Download Action
        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(Directory.GetCurrentDirectory(), Services.Constants.StoragePath, filename);

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
            _services.DeleteFile(filename);

            return RedirectToAction("Index");
        }
    }
}