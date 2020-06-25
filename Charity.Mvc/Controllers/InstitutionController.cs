using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Charity.Mvc.Controllers
{
    public class InstitutionController : Controller
    {
        private readonly IInstitutionService _institutionService;

        public InstitutionController(IInstitutionService institutionService)
        {
            _institutionService = institutionService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> List()
        {
            var model = await _institutionService.GetAllAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEdit(int id = 0)
        {
            if(id == 0)
            return View();
            var model = await _institutionService.GetInstitutionByIdAsync(id);
            if (model == null) return View();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(Institution model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.Id == 0)
            {
                await _institutionService.Add(model);
            }
            else
            {
                await _institutionService.Edit(model);
            }
            return RedirectToAction("List", "Institution");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var institution = await _institutionService.GetInstitutionByIdAsync(id);
            if (institution == null)
                return RedirectToAction("Error", "Home");
            await _institutionService.Remove(institution);
            return RedirectToAction("List");
        }

    }
}
