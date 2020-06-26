using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Pole wymagane")]
        [EmailAddress(ErrorMessage = "Nie poprawny adres email")]
        public string Email { get; set; }
    }
}
