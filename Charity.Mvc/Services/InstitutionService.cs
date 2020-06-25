using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class InstitutionService : IInstitutionService
    {

        private readonly CharityDonationContext _context;

        public InstitutionService(CharityDonationContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Institution model)
        {
            _context.Institutions.Add(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Edit(Institution model)
        {
            _context.Institutions.Update(model);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IList<Institution>> GetAllAsync()
        {
            return await _context.Institutions.ToListAsync();
        }

        public async Task<Institution> GetInstitutionByIdAsync(int id)
        {
            return await _context.Institutions.FindAsync(id);
        }

        public async Task<bool> Remove(Institution model)
        {
            _context.Institutions.Remove(model);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
