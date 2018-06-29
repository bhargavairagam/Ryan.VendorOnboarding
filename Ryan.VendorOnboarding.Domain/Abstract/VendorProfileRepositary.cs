using System;
using System.Collections.Generic;
using System.Text;
using Ryan.VendorOnboarding.Domain.Entities;

namespace Ryan.VendorOnboarding.Domain.Abstract
{
    public interface IVendorProfileRepositary
    {
        IEnumerable<VendorProfile> GetVendorProfiles { get; }

    }
}
