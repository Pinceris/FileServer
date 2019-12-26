using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApp1
{
    public class Services
    {
        public void DeleteFile(string path)
        {
            File.Delete(path);
        }
    }
}
