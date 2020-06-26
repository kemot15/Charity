using Charity.Mvc.Context;
using Charity.Mvc.Models.Db;
using Charity.Mvc.Services.Interfaces;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services
{
    public class AdminService : IAdminService
    {
        private readonly CharityDonationContext _context;

        public AdminService(CharityDonationContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> UserLockAsync(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) return false;
            if (user.LockoutEnabled)
            {
                user.LockoutEnabled = false;
                user.LockoutEnd = null;
            }
            else
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTime.MaxValue;

            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> DeleteUserAsync (User user)
        {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
