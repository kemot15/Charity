using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Charity.Mvc.Controllers
{
    public class DonationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Donate()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Donate(int model)
        {
            return View();
        }
    }
}
