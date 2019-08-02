using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;

namespace Ryan.EmployeeOst.Domain.Abstract
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> Employees { get; }

        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> GetEmployeeByEmpNumberAsync(string empNum);
        Task<IEnumerable<Employee>> GetEmployeesByEmploymentStatus(string empStatus);
        Task<IEnumerable<Employee>> GetEmployeesByEmploymentStatusFirstAndLast(string empStatus, string firstName, string lastName);
    }
}
