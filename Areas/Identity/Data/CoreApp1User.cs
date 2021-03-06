﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoreApp1.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the CoreApp1User class
    public class CoreApp1User : IdentityUser
    {
        [PersonalData]
        public string Nick { get; set; }
        [PersonalData]
        public DateTime DOB { get; set; }
    }
}
