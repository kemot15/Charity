using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Charity.Mvc.Models;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services.Interfaces;

namespace Charity.Mvc.Controllers
{
	public class HomeController : Controller
	{
		private readonly IDonationService _donationService;
		private readonly IInstitutionService _institutionService;

        public HomeController(IDonationService donationService, IInstitutionService institutionService)
        {
            _donationService = donationService;
            _institutionService = institutionService;
        }

        public async Task<IActionResult> Index()
		{
			var model = new MainViewModel
			{
				Institutions = await _institutionService.GetAllAsync(),
				BagQuantity = await _donationService.GetBagsQuantity(),
				InstitutionQuantity = await _donationService.GetSupportedOrganization()
			};
			return View(model);
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
