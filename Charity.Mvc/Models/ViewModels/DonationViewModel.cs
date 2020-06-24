using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.Form;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.ViewModels
{
    public class DonationViewModel : Donation
    {
        //public int Quantity { get; set; }
        //public Institution Institution { get; set; }

        //public string Street { get; set; }
        //public string City { get; set; }
        //public string ZipCode { get; set; }
        //public DateTime PickUpDate { get; set; }
        //public DateTime PickUpTime { get; set; }
        //public string PickUpComment { get; set; }
        public IList<CategoryCheckBoxModel> CategoriesCheckBox { get; set; }

        public IList<InstitutionRadioButton> Institutions { get; set; }

    }
}
