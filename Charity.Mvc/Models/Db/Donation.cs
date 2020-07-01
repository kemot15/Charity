using System;
using System.Collections.Generic;

namespace Charity.Mvc.Models.Db
{
    public class Donation
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public IList<DonationCategory> Categories { get; set; }
        public Institution Institution { get; set; }
        public Status Status { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public string PickUpComment { get; set; }
        public string Phone { get; set; }

        public User User { get; set; }
    }
}


