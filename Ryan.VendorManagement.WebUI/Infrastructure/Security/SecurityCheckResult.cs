using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ryan.VendorManagement.WebUI.Infrastructure.Security
{
    public class SecurityCheckResult
    {
        public bool Valid { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }


        public SecurityCheckResult()
        {
            this.Valid = false;
            this.ActionName = string.Empty;
            this.ControllerName = string.Empty;
        }



    }
}