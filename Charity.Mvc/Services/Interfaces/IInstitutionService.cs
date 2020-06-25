using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services.Interfaces
{
    public interface IInstitutionService
    {
        Task<IList<Institution>> GetAllAsync();
        Task<Institution> GetInstitutionByIdAsync(int id);

        Task<bool> Add(Institution model);
        Task<bool> Edit(Institution model);
        Task<bool> Remove(Institution model);
    }
}
