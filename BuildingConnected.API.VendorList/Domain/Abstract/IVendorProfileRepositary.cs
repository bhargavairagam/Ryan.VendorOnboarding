using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingConnected.API.VendorList.Domain.Entities;

namespace BuildingConnected.API.VendorList.Domain.Abstract
{
    public interface IVendorProfileRepositary
    {
        VendorProfile GetVendorDetials(string username);
        IDbActionResult SaveVendorDetials(VendorProfile vendorProfile);
    }
}
