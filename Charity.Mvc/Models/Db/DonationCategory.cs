using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Db
{
    public class DonationCategory
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int DonationId { get; set; }

    }
}
