using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;

namespace Ryan.EmployeeOst.Domain.Abstract
{
    public interface IRegionRepository
    {
        IEnumerable<Region> Regions { get; }

        Task<IEnumerable<Region>> GetRegionsAsync();
        Task<Region> GetRegionByIDAsync(int id);
        Task<Region> GetRegionByCodeAsync(string regionCode);
    }
}
