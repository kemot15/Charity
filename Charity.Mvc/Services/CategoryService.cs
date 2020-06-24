using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.Form;
using Charity.Mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CharityDonationContext _context;

        public CategoryService(CharityDonationContext context)
        {
            _context = context;
        }
        public async Task<IList<Category>> GetListAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
