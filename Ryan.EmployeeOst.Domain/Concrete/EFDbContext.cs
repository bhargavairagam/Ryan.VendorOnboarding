using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ryan.EmployeeOst.Domain.Entities;

namespace Ryan.EmployeeOst.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("EmployeeOst")
        {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Office> Offices { get; set; }
    }
}
