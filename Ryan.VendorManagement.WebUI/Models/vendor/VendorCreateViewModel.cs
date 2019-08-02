using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ryan.VendorManagement.WebUI.Models.vendor
{
    public class VendorCreateViewModel
    {
        public VendorProfileViewModel VendorProfileViewModel { get; set; }
        public VendorProfileViewModel NonPaidVendorProfileViewModel { get; set; }
        public VendorProfileViewModel CustomerProfileViewModel { get; set; }

    }
}