using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ryan.VendorOnboarding.WebUI.Infrastructure
{
    public static class HelperClass
    {
        public static string InviteSent => "InviteSent";
        public static string InAccouting => "InAccounting";
        public static string Rejected => "Rejected";
        public static string Resend=> "Resend";
        public static string smtpserver => "Relay.ryancompanies.com";
        public static string VendorOnBoardingAdminEmail => "mitch.meilstrup@ryancompanies.com";
    }
}