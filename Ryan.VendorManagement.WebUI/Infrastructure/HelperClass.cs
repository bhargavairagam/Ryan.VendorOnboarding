using Ryan.EmployeeOst.Domain.Concrete;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.VendorOnboarding.Domain.Concrete;
using Ryan.VendorOnboarding.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Ryan.VendorManagement.WebUI.Infrastructure.Security;
using Ryan.VendorOnboarding.Domain;

namespace Ryan.VendorManagement.WebUI.Infrastructure
{
    public static class HelperClass
    {
        
        public static string InviteSent => "InviteSent";
        public static string InAccouting => "InAccounting";
        public static string Resend => "Resend";
        public static string Customer => "C";
        public static string Vendor => "V";
        public static string NonPaid => "N";
        public static string Rejected => "Rejected";
        public static string NotInJDE => "NotInJDE";
        public static string Cancelled => "Cancelled";
        public static string Approved => "Approved";

        // User roles
        public static string ConstructionUser => "ConstructionUser";
        public static string GeneralUser => "GeneralUser";
        public static string RBSREMUSER => "RBSREMUser";
        public static string AdminUser => "AdminUser";
        public static readonly string[] UserTypes = { "AdminUser", "GeneralUser" };

        public static string GetNUllorEmptystring(string val)
        {
            string retval = string.Empty;
            if (string.IsNullOrEmpty(val))
            {
                retval = string.Empty;
            }
            else
            {
                retval = val.Trim();
            }

            return retval;
        }


        public static string ReturnLastFour(string ssn)
        {
            string ss = string.Empty;
            if (!string.IsNullOrEmpty(ssn))
            {
                if( ssn.Length > 4)
                {
                    ss = ssn.Substring(ssn.Length - 4);

                    ssn = ssn.Replace(ss, "XXXX");
                }
                 
            }

            return ss;
        }

        public static string ReturnMaskedSSN(string ssn)
        {
            string ss = string.Empty;
            if (!string.IsNullOrEmpty(ssn))
            {
                if (ssn.Length > 4)
                {
                    ss = ssn.Substring(ssn.Length - 4);

                    ssn = ssn.Replace(ss, "XXXX");
                }

            }

            return ssn;
        }

        public static string MaskSsn(string ssn)
        {
            string result = "";
            if (!string.IsNullOrEmpty(ssn))
            {
                if (ssn.Length > 4)
                {

                    result = ssn.Substring(ssn.Length - 4).PadLeft(ssn.Length, 'X');
                }
            }
           
            return result;

            //char maskCharacter = 'x';
            //int digitsToShow = 4;
            //if (String.IsNullOrWhiteSpace(ssn)) return String.Empty;

            //const int ssnLength = 9;
            //const string separator = "-";
            //int maskLength = ssnLength - digitsToShow;

            //// truncate and convert to number
            //int output = Int32.Parse(ssn.Replace(separator, String.Empty).Substring(maskLength, digitsToShow));

            //string format = String.Empty;
            //for (int i = 0; i < maskLength; i++) format += maskCharacter;
            //for (int i = 0; i < digitsToShow; i++) format += "0";

            //format = format.Insert(3, separator).Insert(6, separator);
            //format = "{0:" + format + "}";

            //return String.Format(format, output);
        }

        public static string ReturnSearchTypeDef(string searchType)
        {
            string ss = string.Empty;
            if (!string.IsNullOrEmpty(searchType))
            {
                if (searchType == "V")
                {
                    ss = "V-Vendor"; 
                }
                else if (searchType == "C")
                {
                    ss = "C-Customer";
                }
                else if (searchType == "N")
                {
                    ss = "N-NonPaid";
                }

            }

            return ss;
        }

        public static IEnumerable<SelectListItem> LoadStates()
        {
            var items = new List<SelectListItem>
        {
            new SelectListItem() {Text = "AL", Value = "AL"},
            new SelectListItem() {Text = "AK", Value = "AK"},
            new SelectListItem() {Text = "AZ", Value = "AZ"},
            new SelectListItem() {Text = "AR", Value = "AR"},
            new SelectListItem() {Text = "CA", Value = "CA"},
            new SelectListItem() {Text = "CO", Value = "CO"},
            new SelectListItem() {Text = "CT", Value = "CT"},
            new SelectListItem() {Text = "DC", Value = "DC"},
            new SelectListItem() {Text = "DE", Value = "DE"},
            new SelectListItem() {Text = "FL", Value = "FL"},
            new SelectListItem() {Text = "GA", Value = "GA"},
            new SelectListItem() {Text = "HI", Value = "HI"},
            new SelectListItem() {Text = "ID", Value = "ID"},
            new SelectListItem() {Text = "IL", Value = "IL"},
            new SelectListItem() {Text = "IN", Value = "IN"},
            new SelectListItem() {Text = "IA", Value = "IA"},
            new SelectListItem() {Text = "KS", Value = "KS"},
            new SelectListItem() {Text = "KY", Value = "KY"},
            new SelectListItem() {Text = "LA", Value = "LA"},
            new SelectListItem() {Text = "ME", Value = "ME"},
            new SelectListItem() {Text = "MD", Value = "MD"},
            new SelectListItem() {Text = "MA", Value = "MA"},
            new SelectListItem() {Text = "MI", Value = "MI"},
            new SelectListItem() {Text = "MN", Value = "MN"},
            new SelectListItem() {Text = "MS", Value = "MS"},
            new SelectListItem() {Text = "MO", Value = "MO"},
            new SelectListItem() {Text = "MT", Value = "MT"},
            new SelectListItem() {Text = "NE", Value = "NE"},
            new SelectListItem() {Text = "NV", Value = "NV"},
            new SelectListItem() {Text = "NH", Value = "NH"},
            new SelectListItem() {Text = "NJ", Value = "NJ"},
            new SelectListItem() {Text = "NM", Value = "NM"},
            new SelectListItem() {Text = "NY", Value = "NY"},
            new SelectListItem() {Text = "NC", Value = "NC"},
            new SelectListItem() {Text = "ND", Value = "ND"},
            new SelectListItem() {Text = "OH", Value = "OH"},
            new SelectListItem() {Text = "OK", Value = "OK"},
            new SelectListItem() {Text = "OR", Value = "OR"},
            new SelectListItem() {Text = "PA", Value = "PA"},
            new SelectListItem() {Text = "RI", Value = "RI"},
            new SelectListItem() {Text = "SC", Value = "SC"},
            new SelectListItem() {Text = "SD", Value = "SD"},
            new SelectListItem() {Text = "TN", Value = "TN"},
            new SelectListItem() {Text = "TX", Value = "TX"},
            new SelectListItem() {Text = "UT", Value = "UT"},
            new SelectListItem() {Text = "VT", Value = "VT"},
            new SelectListItem() {Text = "VA", Value = "VA"},
            new SelectListItem() {Text = "WA", Value = "WA"},
            new SelectListItem() {Text = "WV", Value = "WV"},
            new SelectListItem() {Text = "WI", Value = "WI"},
            new SelectListItem() {Text = "WY", Value = "WY"}
        };
            return items;
        }

        public static IEnumerable<SelectListItem> GetPayementTerms()
        {
            var items = new List<SelectListItem>
        {
            new SelectListItem() {Text = "Default", Value = ""},
            new SelectListItem() {Text = "Due Upon Receipt", Value = "09"},
            new SelectListItem() {Text = "Net 7 Days", Value = "07"},
            new SelectListItem() {Text = "Net 10 Days", Value = "10"},
            new SelectListItem() {Text = "Net 15 Days", Value = "15"},
            new SelectListItem() {Text = "Net 20 Days", Value = "20"},
            new SelectListItem() {Text = "Net 25 Days", Value = "25"},
            new SelectListItem() {Text = "Net 30 Days", Value = "30"},
            new SelectListItem() {Text = "Net 45 Days", Value = "45"},
            new SelectListItem() {Text = "Net 60 Days", Value = "60"},
            new SelectListItem() {Text = "Net 75 Days", Value = "75"},
            new SelectListItem() {Text = "Net 90 Days", Value = "90"}

        };
            return items;
        }

        public static IEnumerable<SelectListItem> GetPackages()
        {
            var items = new List<SelectListItem>
        {

            new SelectListItem() {Text = "MRI1", Value = "MRI1"},
            new SelectListItem() {Text = "MRI2", Value = "MRI2"},
            new SelectListItem() {Text = "MRI10", Value = "MRI10"},
            new SelectListItem() {Text = "MRI12", Value = "MRI12"},
            new SelectListItem() {Text = "YARDI5", Value = "YARDI5"},
            new SelectListItem() {Text = "YARDI6", Value = "YARDI6"},
            new SelectListItem() {Text = "YARDI7", Value = "YARDI7"},
            new SelectListItem() {Text = "YARDI8", Value = "YARDI8"},
            new SelectListItem() {Text = "YARDI11", Value = "YARDI11"},
            new SelectListItem() {Text = "PROCORE", Value = "PROCORE"}
            


        };
            return items;
        }

        public static IEnumerable<SelectListItem> GetPersonCorpCodes()
        {
            var items = new List<SelectListItem>
        {

          new SelectListItem() {Text = "C- C Corp or S Corp", Value = "C"},
            new SelectListItem() {Text = "N - Partnership/LLC", Value = "N"},
             new SelectListItem() {Text = "P - Individual", Value = "P"},
              new SelectListItem() {Text = "G - Govt or Non-Profit", Value = "G"}
        };
            return items;
        }

        public static IEnumerable<SelectListItem> GetJDEVendorStatuses()
        {
            var items = new List<SelectListItem>
        {

            new SelectListItem() {Text = "Active", Value = "Active"},
            new SelectListItem() {Text = "InActive", Value = "InActive"},
             new SelectListItem() {Text = "NotInJDE", Value = "NotInJDE"}
             
        };
            return items;
        }


        public static IEnumerable<SelectListItem> LoadStatuses()
        {
            EFVendorProfileRepositary vendorProfileRepository = new EFVendorProfileRepositary();

            IEnumerable<Status> statuslist = vendorProfileRepository.GetVendorStatuses();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var g in statuslist)
            {
                li.Add(new SelectListItem { Text = g.StatusText, Value = g.StatusValue });
            }

            return li;

        }

        public static async Task<IEnumerable<SelectListItem>> LoadVendorTypes()
        {
            EFVendorTypesRepositary vendorTypesRepository = new EFVendorTypesRepositary();

            IEnumerable<VendorTypes> statuslist = await vendorTypesRepository.GetVendorTypes();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var g in statuslist)
            {
                li.Add(new SelectListItem { Text = g.VENDORTYPETEXT, Value = g.VENDORTYPEVALUE });
            }

            return li;

        }

        public static async Task<IEnumerable<SelectListItem>> LoadJdeStatuses()
        {
            EFJdeStatusesRepositary jdeStatusesRepository = new EFJdeStatusesRepositary();

            IEnumerable<JdeStatuses> statuslist = await jdeStatusesRepository.GetJdeStatuses();
            List<SelectListItem> li = new List<SelectListItem>();

            foreach (var g in statuslist)
            {
                li.Add(new SelectListItem { Text = g.JDESTATUSTEXT, Value = g.JDESTATUSVALUE });
            }

            return li;

        }

        public static async Task<bool> SaveStatuHistoryRecord(string Description , string status   , int id)
        {
            EFVendorStatusHistory statusHistoryRepo = new EFVendorStatusHistory();
            string username = Convert.ToString(HttpContext.Current.Session["username"]);

            StatusHistory sh = new StatusHistory();
            sh.UpdatedBy = username;
            sh.UpdatedTime = DateTime.Now;
            sh.Description = Description;
            sh.VStatus = HelperClass.InviteSent;
            sh.VID = id;


            EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);

            if(!saveStatusHist.Succeeded )
            {
                return false;
            }
            return true;

        }





        }
}