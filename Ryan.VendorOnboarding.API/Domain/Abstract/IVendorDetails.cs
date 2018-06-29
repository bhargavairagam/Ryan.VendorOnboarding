using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ryan.VendorOnboarding.API.Domain.Entities;

namespace Ryan.VendorOnboarding.API.Domain.VendorDetails
{
    public interface IVendorDetailsRepositary
    {
        Task<IEnumerable<VendorProfile>> GetVendorProfiles();
        Task<VendorProfile> GetVendorDetials(string username);
        Task<IDbActionResult> SaveVendorDetials(VendorProfile vendorProfile);

    }
}