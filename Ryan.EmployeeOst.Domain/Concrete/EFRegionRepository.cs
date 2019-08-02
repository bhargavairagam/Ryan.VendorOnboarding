using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Abstract;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFRegionRepository : IRegionRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Region> Regions
        {
            get { return context.Regions.ToList(); }
        }

        public async Task<IEnumerable<Region>> GetRegionsAsync()
        {
            return await Task<IEnumerable<Region>>.Factory.StartNew(() =>
            {
                return context.Regions.ToList();
            });
        }

        public async Task<Region> GetRegionByIDAsync(int id)
        {
            return await context.Regions.FindAsync(id);
        }

        public async Task<Region> GetRegionByCodeAsync(string regionCode)
        {
            return await Task<Region>.Factory.StartNew(() =>
            {
                return context.Regions.FirstOrDefault(r => r.RegionCode == regionCode);
            });
        }
    }
}
