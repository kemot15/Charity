using Charity.Mvc.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Charity.Mvc.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IList<Category>> GetListAsync();
    }
}
