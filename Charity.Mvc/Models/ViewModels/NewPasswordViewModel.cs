using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class NewPasswordViewModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [Compare("Password", ErrorMessage = "Podane hasła są różne")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
