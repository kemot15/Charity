using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Db
{
    public class Institution
    {
        public int Id { get; set; }
        
        [DisplayName("Nazwa")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Opis")]
        [Required]
        public string Description { get; set; }
    }
}
