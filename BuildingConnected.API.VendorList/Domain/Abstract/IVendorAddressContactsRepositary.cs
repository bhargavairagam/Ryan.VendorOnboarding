using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingConnected.API.VendorList.Domain.Entities;

namespace BuildingConnected.API.VendorList.Domain.Abstract
{
    public interface IVendorAddressContactsRepositary
    {
        VendorAddressContacts GetVendorAddressContacts(string username);
        IDbActionResult SaveVendorAddressContacts(VendorAddressContacts vendorProfile);

        Task<IDbActionResult> SaveVendorAddressContactsAsync(VendorAddressContacts vendorAddCont);
    }
}
