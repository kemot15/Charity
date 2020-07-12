using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.Form;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Charity.Mvc.Controllers
{
    public class DonationController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IInstitutionService _institutionService;
        private readonly IDonationService _donationService;
        private readonly UserManager<User> UserManaget;

        public DonationController(ICategoryService categoryService, IInstitutionService institutionService, IDonationService donationService, UserManager<User> userManaget)
        {
            _categoryService = categoryService;
            _institutionService = institutionService;
            _donationService = donationService;
            UserManaget = userManaget;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Donate()
        {
            var categoriesList = await _categoryService.GetListAsync();
            var categoryCheckBox = new List<CategoryCheckBoxModel>();
            foreach (var category in categoriesList)
            {
                categoryCheckBox.Add(new CategoryCheckBoxModel
                {
                    Id = category.Id,
                    Text = category.Name,
                    //IsChecked = false
                });
            }

            var institutionList = await _institutionService.GetAllAsync();
            var institutionRadioButton = new List<InstitutionRadioButton>();
            if (institutionList == null)
            {
                institutionRadioButton.Add(new InstitutionRadioButton
                {
                    Id = 0,
                    Name = "Brak instytucji",
                    Description = "",
                   // IsChecked = false
                });
            }else
            {
                foreach (var institution in institutionList)
                {
                    institutionRadioButton.Add(new InstitutionRadioButton
                    {
                        Id = institution.Id,
                        Name = institution.Name,
                        Description = institution.Description,
                        IsChecked = false
                    });
                }
            }            
            var model = new DonationViewModel
            {
                CategoriesCheckBox = categoryCheckBox,
                Institutions = institutionRadioButton,
                PickUpDate = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Donate(DonationViewModel model)
        {
            var user = await UserManaget.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            if (model == null) return View(model);
            var categories = model.CategoriesCheckBox.Where(c => c.IsChecked == true).ToList();
            var categoriesList = new List<DonationCategory>();
            foreach(var category in categories)
            {
                categoriesList.Add(new DonationCategory
                {
                    CategoryId = category.Id,
                    DonationId = model.Id
                });
            }
          

            var donation = new Donation
            {
                Quantity = model.Quantity,
                Categories = categoriesList,
                Institution = await _institutionService.GetInstitutionByIdAsync(model.Institution.Id),
                Street = model.Street,
                City = model.City,
                ZipCode = model.ZipCode,
                PickUpDate = model.PickUpDate,
                PickUpTime = model.PickUpTime,
                PickUpComment = model.PickUpComment,
                Phone = model.Phone,
                Status = Status.Deposited,
                User = user
            };
            await _donationService.AddDonation(donation);
            return RedirectToAction("FormConfirmation");
        }

        public IActionResult FormConfirmation()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var user = await UserManaget.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");
            var model = User.IsInRole("Admin") ? await _donationService.GetAllAsync() : await _donationService.GetAllUserAsync(user);
            return View(model);
        }

        public async Task<IActionResult> DonationDetail(int id)
        {
            
            var model = await _donationService.GetDonationByIdAsync(id);
            if (model == null) return RedirectToAction("List", "Donation");
            return View(model);
        }
    }    
}
