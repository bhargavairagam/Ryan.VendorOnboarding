using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Abstract;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFJobRepository : IJobRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Job> Jobs
        {
            get
            {
                List<Job> jobs = new List<Job>();

                var rawResults = context.Employees.Select(j => new { j.JobCode, j.JobDesc }).Distinct();

                foreach (var resultItem in rawResults)
                {
                    jobs.Add(new Job(resultItem.JobCode, resultItem.JobDesc));
                }

                return jobs;
            }
        }
    }
}
