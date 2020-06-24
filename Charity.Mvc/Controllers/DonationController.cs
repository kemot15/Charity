using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.Form;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Charity.Mvc.Controllers
{
    public class DonationController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IInstitutionService _institutionService;

        public DonationController(ICategoryService categoryService, IInstitutionService institutionService)
        {
            _categoryService = categoryService;
            _institutionService = institutionService;
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
                    IsChecked = true
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
                    Desription = "",
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
                        Desription = institution.Description,
                        IsChecked = false
                    });
                }
            }            
            var model = new DonationViewModel
            {
                Categories = categoryCheckBox,
                Institutions = institutionRadioButton
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Donate(DonationViewModel model)
        {
            if (model == null) return View();

            return View();
        }
    }
}
