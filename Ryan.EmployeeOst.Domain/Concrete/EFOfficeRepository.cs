using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Abstract;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFOfficeRepository : IOfficeRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Office> Offices
        {
            get { return context.Offices.ToList(); }
        }

        public async Task<IEnumerable<Office>> GetOfficesAsync()
        {
            return await Task<IEnumerable<Office>>.Factory.StartNew(() =>
            {
                return context.Offices.ToList();
            });
        }

        public async Task<Office> GetOfficeByIDAsync(int id)
        {
            return await context.Offices.FindAsync(id);
        }

        public async Task<Office> GetOfficeByCodeAsync(string officeCode)
        {
            return await Task<Office>.Factory.StartNew(() =>
            {
                return context.Offices.FirstOrDefault(o => o.OfficeCode == officeCode);
            });
        }

        public async Task<IEnumerable<Office>> GetOfficesByRegionCode(string regionCode)
        {
            return await Task<IEnumerable<Office>>.Factory.StartNew(() =>
            {
                return context.Offices.Where(o => o.RegionCode == regionCode).ToList();
            });
        }
    }
}
