using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Abstract;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFCostCenterRepository : ICostCenterRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<CostCenter> CostCenters
        {
            get
            {
                List<CostCenter> costCenters = new List<CostCenter>();

                var rawResults = context.Employees.Select(c => new { c.HomeBusUnit, c.HomeBusUnitDesc }).Distinct();

                foreach (var resultItem in rawResults)
                {
                    costCenters.Add(new CostCenter(resultItem.HomeBusUnit, resultItem.HomeBusUnitDesc));
                }

                return costCenters;
            }
        }

        public async Task<IEnumerable<CostCenter>> GetCostCentersAsync()
        {
            return await Task<IEnumerable<CostCenter>>.Factory.StartNew(() =>
            {
                List<CostCenter> costCenters = new List<CostCenter>();

                var rawResults = context.Employees.Select(c => new { c.HomeBusUnit, c.HomeBusUnitDesc }).Distinct();

                foreach (var resultItem in rawResults)
                {
                    costCenters.Add(new CostCenter(resultItem.HomeBusUnit, resultItem.HomeBusUnitDesc));
                }

                return costCenters;
            });
        }

        public IEnumerable<CostCenter> GetFilteredCostCentersList(string searchTerm)
        {
            List<CostCenter> costCenters = new List<CostCenter>();

            var rawResults = context.Employees.Where(c => c.HomeBusUnit.Contains(searchTerm) || c.HomeBusUnitDesc.Contains(searchTerm)).Select(c => new { c.HomeBusUnit, c.HomeBusUnitDesc }).Distinct().OrderBy(c => c.HomeBusUnit).ThenBy(c => c.HomeBusUnitDesc);

            foreach (var resultItem in rawResults)
            {
                costCenters.Add(new CostCenter(resultItem.HomeBusUnit, resultItem.HomeBusUnitDesc));
            }

            return costCenters;
        }
    }
}
