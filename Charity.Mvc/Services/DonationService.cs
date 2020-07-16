using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Models.ViewModels;
using Charity.Mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            return await _context.Donations.Include(i => i.Institution).ToListAsync();
        }
        public async Task<IList<Donation>> GetAllUserAsync(User user)
        {
            return await _context.Donations.Include(i => i.Institution).Where(d => d.User == user).ToListAsync();
        }

        public async Task<int> GetBagsQuantity()
        {
            return await _context.Donations.SumAsync(b => b.Quantity);
        }

        public async Task<int> GetSupportedOrganization()
        {
            return await _context.Donations.Select(i => i.Institution).Distinct().CountAsync();
        }

        public async Task<bool> AddDonation(Donation donation)
        {
            _context.Add(donation);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Donation> GetDonationByIdAsync(int id)
        {
            return await _context.Donations.Include(i => i.Institution).SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> UpdateDonationStatusAsync(Donation donation)
        {
            _context.Donations.Update(donation);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
