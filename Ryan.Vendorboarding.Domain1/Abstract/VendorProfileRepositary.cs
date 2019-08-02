
using System.Collections.Generic;
using System.Threading.Tasks;
using Ryan.VendorOnboarding.Domain;
using Ryan.VendorOnboarding.Domain.Entities;

namespace Ryan.VendorOnboarding.Domain.Abstract
{
    public interface IVendorProfileRepositary
    {
        Task<IEnumerable<VendorProfile>> GetVendorProfiles();
        Task<VendorProfile> GetVendorDetials(string ein);
        Task<EFDbActionResult> SaveVendorDetials(VendorProfile vendorProfile);


    }

    public interface IStatusHistoryRepositary
    {
        
        Task<IEnumerable<StatusHistory>> GetStatusHistoryByID(int vid);
        Task<EFDbActionResult> SaveStatusHistoryByID(StatusHistory statushistory);


    }
}
