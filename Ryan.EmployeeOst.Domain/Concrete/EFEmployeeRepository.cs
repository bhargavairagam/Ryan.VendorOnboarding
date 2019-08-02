using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Abstract;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private EFDbContext context = new EFDbContext();

        public IEnumerable<Employee> Employees
        {
            get { return context.Employees.ToList(); }
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await Task<IEnumerable<Employee>>.Factory.StartNew(() =>
            {
                return context.Employees.ToList();
            });
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await context.Employees.FindAsync(id);
        }

        public async Task<Employee> GetEmployeeByEmpNumberAsync(string empNum)
        {
            return await Task<Employee>.Factory.StartNew(() =>
            {
                return context.Employees.FirstOrDefault(e => e.EmpNumber.ToLower() == empNum.ToLower());
            });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByEmploymentStatus(string empStatus)
        {
            return await Task<IEnumerable<Employee>>.Factory.StartNew(() =>
            {
                return context.Employees.Where(e => e.EmpStatus.ToLower() == empStatus.ToLower()).OrderBy(e => e.FullName).ToList();
            });
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByEmploymentStatusFirstAndLast(string empStatus, string firstName, string lastName)
        {
            
            return await Task<IEnumerable<Employee>>.Factory.StartNew(() =>
            {

                List<Employee> results = new List<Employee>();

                string searchType = GetSearchTypeBasedOnFirstAndLastParams(firstName, lastName);

                switch (searchType)
                {
                    case SEARCH_TYPE_BOTH:
                        results = context.Employees.Where(e => e.EmpStatus.ToLower() == empStatus.ToLower() && e.First.ToLower().StartsWith(firstName.ToLower()) && e.Last.ToLower().StartsWith(lastName.ToLower())).OrderBy(e => e.FullName).ToList();
                        break;
                    case SEARCH_TYPE_FIRST_ONLY:
                        results = context.Employees.Where(e => e.EmpStatus.ToLower() == empStatus.ToLower() && e.First.ToLower().StartsWith(firstName.ToLower())).OrderBy(e => e.FullName).ToList();
                        break;
                    case SEARCH_TYPE_LAST_ONLY:
                        results = context.Employees.Where(e => e.EmpStatus.ToLower() == empStatus.ToLower() && e.Last.ToLower().StartsWith(lastName.ToLower())).OrderBy(e => e.FullName).ToList();
                        break;
                    default:
                        results = context.Employees.Where(e => e.EmpStatus.ToLower() == empStatus.ToLower()).OrderBy(e => e.FullName).ToList();
                        break;
                }
                return results;
            });
        }

        private string GetSearchTypeBasedOnFirstAndLastParams(string first, string last)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(first))
            {
                if (string.IsNullOrEmpty(last))
                {
                    result = SEARCH_TYPE_NEITHER;
                }
                else
                {
                    result = SEARCH_TYPE_LAST_ONLY;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(last))
                {
                    result = SEARCH_TYPE_FIRST_ONLY;
                }
                else
                {
                    result = SEARCH_TYPE_BOTH;
                }
            }

            return result;
        }

        private const string SEARCH_TYPE_FIRST_ONLY = "FIRST-ONLY";
        private const string SEARCH_TYPE_LAST_ONLY = "LAST-ONLY";
        private const string SEARCH_TYPE_BOTH = "BOTH";
        private const string SEARCH_TYPE_NEITHER = "NEITHER";
    }
}
