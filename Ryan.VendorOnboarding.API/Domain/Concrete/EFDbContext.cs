using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Ryan.VendorOnboarding.API.Domain.Entities;


namespace Ryan.VendorOnboarding.API.Domain.Concrete
{
    public class EFDbContext: DbContext
    {

        public EFDbContext() : base("VendorOnboardingConn")
        {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<VendorProfile> VendorProfiles { get; set; }

    }
}