using System;
using System.Data.Entity;
using Ryan.VendorOnboarding.Domain.Entities;

namespace Ryan.VendorOnboarding.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public EFDbContext() : base("VendorOnboardingConn")
        {
            Database.SetInitializer<EFDbContext>(null);
        }

        public DbSet<VendorProfile> VendorProfiles { get; set; }
    }
}
