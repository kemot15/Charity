using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class MainViewModel
    {
        //public IList<Donation> Donations { get; set; }
        public IList<Institution> Institutions { get; set; }
        public int BagQuantity { get; set; }
        public int InstitutionQuantity { get; set; }
    }
}
