using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Models.Form
{
    public class CategoryCheckBoxModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsChecked { get; set; }
    }
}
