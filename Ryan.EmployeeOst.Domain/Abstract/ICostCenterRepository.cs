using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;

namespace Ryan.EmployeeOst.Domain.Abstract
{
    public interface ICostCenterRepository
    {
        IEnumerable<CostCenter> CostCenters { get; }

        Task<IEnumerable<CostCenter>> GetCostCentersAsync();
        IEnumerable<CostCenter> GetFilteredCostCentersList(string searchTerm);
    }
}
