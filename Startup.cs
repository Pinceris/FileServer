using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreApp1.Areas.Identity.Data;
using CoreApp1.Areas.Identity.Services;
using CoreApp1.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApp1
{
    public class Startup
    {
        //a list of Uploaded files
        public static List<FileUploadModel> fileUploadModels = GetFiles();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<UserContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // requires
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using WebPWrecover.Services;
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private static List<FileUploadModel> GetFiles()
        {
            List<FileUploadModel> files = new List<FileUploadModel>();

            List<FileUploadModel> fileUploads;

            using (var ctx = new FileUploadContext())
            {
                fileUploads = ctx.Files.AsEnumerable<FileUploadModel>().ToList();
            }

            string[] filesInDir = Directory.GetFiles("FileStorage");

            if (filesInDir.Length > 0)
            {
                foreach (string file in filesInDir)
                {
                    FileUploadModel fileUpload;

                    string fileName = file.Split('\\')[1];

                    if(fileUploads.Any(f => f.FileName == fileName))
                    {
                        fileUpload = fileUploads.Where(f => f.FileName == fileName).FirstOrDefault();
                    }
                    else
                    {
                        fileUpload = new FileUploadModel()
                        {
                            Id = Guid.NewGuid(),
                            Author = "System",
                            FileName = fileName,
                            Created_At = DateTime.Now,
                            Downloads = 0
                        };
                    }

                    files.Add(fileUpload);
                }
            }
            return files;
        }
    }
}
