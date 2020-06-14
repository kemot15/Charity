using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IList<Institution>> GetAllAsync()
        {
            return await _context.Institutions.ToListAsync();
        }
    }
}
