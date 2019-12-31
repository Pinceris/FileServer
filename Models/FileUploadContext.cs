using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp1.Models
{
    public class FileUploadContext : DbContext
    {
        public DbSet<FileUploadModel> Files { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-L79A5C6;Database=FileServerDB;Trusted_Connection=True;");
        }
    }
}
