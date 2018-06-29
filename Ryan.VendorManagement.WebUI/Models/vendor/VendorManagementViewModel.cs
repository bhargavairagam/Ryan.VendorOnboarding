using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ryan.VendorManagement.WebUI.Models.vendor
{
    public class VendorManagementViewModel
    {
        public string VendorEIN { get; set; }
        public string VendorLegalName { get; set; }
        public string VendorEmail { get; set; }
        public string VendorGuid { get; set; }
    }
}