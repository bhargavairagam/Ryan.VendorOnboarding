using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;


namespace Ryan.EmployeeOst.Domain.Abstract
{
    public interface IOfficeRepository
    {
        IEnumerable<Office> Offices { get; }

        Task<IEnumerable<Office>> GetOfficesAsync();
        Task<Office> GetOfficeByIDAsync(int id);
        Task<Office> GetOfficeByCodeAsync(string officeCode);
        Task<IEnumerable<Office>> GetOfficesByRegionCode(string regionCode);

    }
}
