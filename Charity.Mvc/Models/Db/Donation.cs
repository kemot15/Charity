using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Db
{
    public class Donation
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public IList<Category> Categories { get; set; }
        public Institution Institution { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public string PickUpComment { get; set; }
    }
}


