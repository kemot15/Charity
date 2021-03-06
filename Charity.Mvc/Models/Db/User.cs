﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Db
{
    public class User : IdentityUser<int>
    {
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }
        [DisplayName("Nazwisko"), MaxLength(255)]
        public string LastName { get; set; }
    }
}
