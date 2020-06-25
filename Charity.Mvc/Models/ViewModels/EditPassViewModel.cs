﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Charity.Mvc.Models.ViewModels
{
    public class EditPassViewModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [DataType(DataType.Password)]
        [DisplayName("Stare hasło")]
        public string OldPass { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DataType(DataType.Password)]
        [DisplayName("Nowe hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole wymagane"), Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        [DataType(DataType.Password)]
        [DisplayName("Powtórz nowe hasło")]
        public string ReapeatPassword { get; set; }
    }
}
