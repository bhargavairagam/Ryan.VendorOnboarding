using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.DirectoryServices;
using Ryan.EmployeeOst.Domain.Entities;
using Ryan.EmployeeOst.Domain.Concrete;
using Ryan.VendorOnboarding.Domain.Concrete;
using Ryan.VendorOnboarding.Domain.Entities;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Ryan.VendorManagement.WebUI.Infrastructure.Security
{
    public class SecurityUtility
    {
        /// <summary>
        /// Strips out the domain name and just returns the Username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetSimpleUserName(IPrincipal user)
        {
            string username = Convert.ToString(HttpContext.Current.Session["username"]);

            if(username == "")
            {
                username = user.Identity.Name.Replace(@"RYAN\", "");
                HttpContext.Current.Session["username"] = username;
            }
           

            return username;
        }

        public static  string CheckUserRole(IPrincipal user)
        {
            string userrole = Convert.ToString(HttpContext.Current.Session["userrole"]);
            VendorAppRole vrole = new VendorAppRole();
            if (userrole == string.Empty)
            {
                vrole =  GetVendorOnboardingAppRole(user).Result;

                HttpContext.Current.Session["userrole"] = vrole.UserRole;
                HttpContext.Current.Session["usertype"] = vrole.UserType;
                userrole = vrole.UserRole;


            }

            return userrole;
        }

        public static string CheckUserType(IPrincipal user)
        {
            string usertype = Convert.ToString(HttpContext.Current.Session["usertype"]);
            VendorAppRole vrole = new VendorAppRole();
            if (usertype == string.Empty)
            {
                vrole = GetVendorOnboardingAppRole(user).Result;

                HttpContext.Current.Session["userrole"] = vrole.UserRole;
                HttpContext.Current.Session["usertype"] = vrole.UserType;
                usertype = vrole.UserRole;


            }

            return usertype;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleToCheck"></param>
        /// <returns></returns>
        public static async Task<SecurityCheckResult> CheckUserRights(IPrincipal user, string roleToCheck)
        {
            SecurityCheckResult result = new SecurityCheckResult();
            string usersRole = Convert.ToString(HttpContext.Current.Session["userrole"]);

            string usertype = Convert.ToString(HttpContext.Current.Session["usertype"]);
            VendorAppRole vrole = new VendorAppRole();

            if (usertype == string.Empty)
            {
                vrole = await GetVendorOnboardingAppRole(user);

                HttpContext.Current.Session["userrole"] = vrole.UserRole;
                HttpContext.Current.Session["usertype"] = vrole.UserType;
                usersRole = vrole.UserRole;
                usertype = vrole.UserType;
            }

            string[] allusertypes = roleToCheck.Split(',');

            if (allusertypes.Contains(usertype))
            {
                result.Valid = true;
            }
            else
            {
                result.Valid = false;
                // If user's role doesn't match the role to check, then
                // redirect them to the index of the appropriate role.

                switch (usersRole)
                {
                    case "GeneralUser":
                        result.ControllerName = "VendorManagement";
                        result.ActionName = "Index";
                        break;
                    case "ConstructionUser":
                        result.ControllerName = "VendorManagement";
                        result.ActionName = "Index";
                        break;
                    case "REMRBSUser":
                        result.ControllerName = "VendorManagement";
                        result.ActionName = "Index";
                        break;
                    case "AdminUser":
                        result.ControllerName = "Admin";
                        result.ActionName = "Index";
                        break;
                    default:
                        result.ControllerName = "VendorManagement";
                        result.ActionName = "NoAccess";
                        break;
                }
            }

            return result;
        }

        public static async Task<VendorAppRole> GetVendorOnboardingAppRole(IPrincipal user)
        {
            string overrideValue = string.Empty;

            EFVendorAppRoleRepositary vapp = new EFVendorAppRoleRepositary();

            VendorAppRole vrole = new VendorAppRole();
            try
            {
                vrole =  vapp.GetVendorRoleByName(user.Identity.Name.Replace(@"RYAN\", "").ToLower());
            }
            catch(Exception ex)
            {

            }
            

            if(vrole != null)
            {
                return vrole;
            }

            return GetVendorOnboardingAppRole(user, overrideValue);
        }


        public static VendorAppRole GetVendorOnboardingAppRole(IPrincipal user, string overrideRole)
        {
            string role = "GeneralUser";
            VendorAppRole vrole = new VendorAppRole();
            vrole.Username = user.Identity.Name.Replace(@"RYAN\", "").ToLower();
            EventLog v = new EventLog("Application");

            if (!string.IsNullOrEmpty(overrideRole))
            {
                vrole.UserRole = role;
                vrole.UserType = role;
                
            }
            else
            {
                

                // Check for membership to the following AD groups. // Construction team
                if (user.IsInRole(@"RYAN\RYAN-PROCORE-ALL") )
                {
                    vrole.UserRole = HelperClass.ConstructionUser;
                    vrole.UserType = role;
                   
                }
                else if (user.IsInRole(@"RYAN\MIN-PROP RBS") || user.IsInRole(@"RYAN\MIN-PROP MGMT MGRS"))   //  REM : MIN-PROP MGMT MGRS
                {
                    vrole.UserRole = HelperClass.RBSREMUSER;
                    vrole.UserType = role;  //  RBS User
                }
                else 
                {
                    vrole.UserRole = HelperClass.GeneralUser;
                    vrole.UserType = role;
                }
            }

            return vrole;
        }

        /// <summary>
        /// Strips out the domain name and just returns the Username
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserEmail(IPrincipal user)
        {
            string name = GetSimpleUserName(user);

            UserDetails det = new UserDetails();
            // string name = user.Identity.Name.Replace(@"RYAN\", "");
            
            string email = Convert.ToString(HttpContext.Current.Session["email"]);
            if ( email == "")
            {
                EFEmployeeRepository emprepo = new EFEmployeeRepository();
                Employee emp = emprepo.Employees.FirstOrDefault(x => x.LoginID == name);
                if (emp != null)
                {
                    det.FirstName = emp.First;
                    det.LastName = emp.Last;
                    det.UserEmail  = emp.Email ?? "";
                    email = emp.Email ?? "";
                    HttpContext.Current.Session["email"] = email;
                }
                return email;
            }
            else
                return email ?? "";
            
        }
    }
}