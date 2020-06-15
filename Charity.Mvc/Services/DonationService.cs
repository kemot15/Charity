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
    public class DonationService : IDonationService
    {
        private readonly CharityDonationContext _context;

        public DonationService(CharityDonationContext context)
        {
            _context = context;
        }

        public async Task<IList<Donation>> GetAllAsync()
        {
            return await _context.Donations.ToListAsync();
        }

        public async Task<int> GetBagsQuantity()
        {
            return await _context.Donations.SumAsync(b => b.Quantity);
        }

        public async Task<int> GetSupportedOrganization()
        {
            return await _context.Donations.Select(i => i.Institution).Distinct().CountAsync();
        }
    }
}
