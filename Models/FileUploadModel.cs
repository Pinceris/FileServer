using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp1.Models
{
    public class FileUploadModel
    {
        public Guid Id { get; set; }
        [DisplayName("File")]
        public string FileName { get; set; }
        [DisplayName("Upload Date")]
        public DateTime Created_At { get; set; }
        public int Downloads { get; set; }
    }
}
