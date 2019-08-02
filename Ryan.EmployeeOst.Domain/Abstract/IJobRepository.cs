using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;

namespace Ryan.EmployeeOst.Domain.Abstract
{
    public interface IJobRepository
    {
        IEnumerable<Job> Jobs { get; }

    }
}
