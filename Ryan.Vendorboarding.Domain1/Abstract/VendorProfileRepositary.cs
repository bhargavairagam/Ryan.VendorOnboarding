
using System.Collections.Generic;
using System.Threading.Tasks;
using Ryan.Vendorboarding.Domain;
using Ryan.VendorOnboarding.Domain.Entities;

namespace Ryan.VendorOnboarding.Domain.Abstract
{
    public interface IVendorProfileRepositary
    {
        Task<IEnumerable<VendorProfile>> GetVendorProfiles();
        Task<VendorProfile> GetVendorDetials(string ein);
        Task<IDbActionResult> SaveVendorDetials(VendorProfile vendorProfile);


    }
}
