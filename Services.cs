using CoreApp1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp1
{
    public class Services
    {
        public struct Constants
        {
            public const string AlertString = "<script language = 'javascript' type='text/javascript'>$(document).ready(function () { alert('Please confirm email to Upload Files'); });</script>";
            //folder where uploads are kept
            public const string StoragePath = "FileStorage";
        }
        public void DeleteFile(string filename)
        {
            if (filename != null)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), Constants.StoragePath, filename);

                File.Delete(path);
                Startup.fileUploadModels.RemoveAll(f => f.FileName == filename);
            }      
        }
        public void UploadFiles(List<IFormFile> files, string name)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.Join(Constants.StoragePath, formFile.FileName);
                    //Save File Info
                    FileUploadModel fileUploadModel = new FileUploadModel()
                    {
                        Id = Guid.NewGuid(),
                        Author = name,
                        FileName = formFile.FileName,
                        Created_At = DateTime.Now,
                        Downloads = 0
                    };
                    Startup.fileUploadModels.Add(fileUploadModel);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                }
            }
        }
    }
}
