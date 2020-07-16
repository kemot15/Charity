using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services.Interfaces
{
    public interface IDonationService
    {
        Task<IList<Donation>> GetAllAsync();
        Task<int> GetBagsQuantity();

        Task<int> GetSupportedOrganization();
        Task<bool> AddDonation(Donation donation);
        Task<Donation> GetDonationByIdAsync(int id);
        Task<IList<Donation>> GetAllUserAsync(User user);

        Task<bool> UpdateDonationStatusAsync(Donation donation);
    }
}
