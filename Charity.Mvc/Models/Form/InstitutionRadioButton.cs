using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Form
{
    public class InstitutionRadioButton : Institution
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Desription { get; set; }
        public bool IsChecked { get; set; }
    }
}
