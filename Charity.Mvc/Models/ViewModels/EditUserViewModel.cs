using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Imię"), MaxLength(255)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
        [DisplayName("Nazwisko"), MaxLength(255)]
        public string LastName { get; set; }

    }
}
