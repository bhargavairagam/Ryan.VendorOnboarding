using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingConnected.API.VendorList.Domain.Entities;

namespace BuildingConnected.API.VendorList.Domain.Concrete
{
    public class EFDbContext : DbContext
    {

        public EFDbContext() : base("VendorOnboardingConn")
        {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<VendorProfile> VendorProfiles { get; set; }
        public DbSet<VendorAddressContacts> VendorAddressContacts { get; set; }

    }
}
