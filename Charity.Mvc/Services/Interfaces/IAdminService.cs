using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services.Interfaces
{
    public interface IAdminService
    {
        Task<bool> UserLockAsync(int id);
        Task<User> GetUserAsync(int id);
        Task<bool> DeleteUserAsync(User user);
        int CountUsersInRoles(int roleId);
    }
}
