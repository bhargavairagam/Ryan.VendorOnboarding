using Ryan.VendorManagement.WebUI.Models.vendor;
using Ryan.VendorOnboarding.Domain.Entities;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Web.Mvc;
using Ryan.VendorOnboarding.Domain.Concrete;
using Ryan.VendorManagement.WebUI.Infrastructure;
using Ryan.VendorManagement.WebUI.Infrastructure.Security;

namespace Ryan.VendorManagement.WebUI.Controllers
{
    public class AdminController : Controller
    {

        EFVendorProfileRepositary vendorProfileRepository = new EFVendorProfileRepositary();
        SecurityCheckResult securityCheck = null;
        // GET: Admin

        public async Task<ActionResult> Index() {

             securityCheck = await SecurityUtility.CheckUserRights(User, "AdminUser");

            if (!securityCheck.Valid)
            {
                return RedirectToAction(securityCheck.ActionName, securityCheck.ControllerName);
            }

            return RedirectToAction("VendorStatus");
        }

        [HttpPost]
        public async Task<ActionResult>  Index(string SearchText,string SearchBy)
        {
            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorListByName(SearchText.ToLower());

          
            List<VendorProfile> prof = new List<VendorProfile> () ; 
             ViewBag.CurrentFilter = SearchText;

            if (!string.IsNullOrEmpty(SearchText))
            {
                if (SearchBy.ToString() == "true") {
                    // starts with is true
                    prof = profiles.Where(x => x.VendorDBAName.ToLower().StartsWith(SearchText.ToLower())).ToList();
                    
                }
                else
                {
                    prof = profiles.Where(x => x.VendorDBAName.ToLower().Contains(SearchText.ToLower())).ToList();
                }
              
               // prof = profiles.ToList();
            }
             
         

            foreach (VendorProfile pro in prof)
            {
                VendorProfileViewModel v = new VendorProfileViewModel();
                v.VendorLegalName = pro.VendorLegalName;
                v.VendorEIN = pro.VendorEIN;
                v.VendorStatus = pro.VendorStatus;
                v.EmailAddress = pro.VendorEmail;
                v.ID = pro.ID;
                v.JDEVendorID = pro.JDEVendorID;
                v.VendorAddress1 = pro.VendorAddress1;
                v.VendorAddress2 = pro.VendorAddress2;
                v.VendorCity = pro.VendorCity;
                v.VendorState = pro.VendorState;
                v.VendorStatus = pro.VendorStatus;
                v.VendorEmail = pro.VendorEmail;
                v.VendorDBAName = pro.VendorDBAName;
                
                vendorProfiles.Add(v);

            }
            //int num_of_page = (pageNum ?? 1);
            // return View(vendorProfiles.ToPagedList(num_of_page, pagesize));
            return View(vendorProfiles);
            //return View();
        }

        [HttpGet]
        public ActionResult VendorStatus()
        {
            if (securityCheck is null)
            {
                securityCheck = SecurityUtility.CheckUserRights(User, "AdminUser").Result;

                if (!securityCheck.Valid)
                {
                    return RedirectToAction(securityCheck.ActionName, securityCheck.ControllerName);
                }
            }

            return View();
        }


        [HttpGet]
        public ActionResult ViewAllVendors()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ExportData()
        {
            ViewBag.Packages = HelperClass.GetPackages();
            ViewBag.Statuses = HelperClass.LoadStatuses();
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetVendorByAdvanceSearch( string status , string package)
        {
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
            profiles = profiles.Where(x => x.VendorEIN != null).ToList();

          //  dynamic pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.VendorEIN, i.JDEVendorID, i.VendorStatus, i.VendorAddress1, i.VendorCity, i.VendorState, i.VendorZipCode, i.VendorEmail, i.VendorContactFN, i.VendorContactLN, i.PaymentTerm, i.PersonCorpCode, i.MRI1, i.MRI2, i.MRI10, i.MRI12, i.YARDI5, i.YARDI6, i.YARDI7, i.YARDI8, i.YARDI11, i.LastUpdatedTime }).OrderByDescending(x => x.LastUpdatedTime).ToList();
          // string vendstatus = "";

            if (package == "MRI1")
            {
                profiles = profiles.Where(x => x.MRI1 == "Y");
            }
            else if (package == "MRI2")
            {
                profiles = profiles.Where(x => x.MRI2 == "Y");
            }
            else if (package == "MRI10")
            {
                profiles = profiles.Where(x => x.MRI10 == "Y");
            }
            else if (package == "MRI12")
            {
                profiles = profiles.Where(x => x.MRI12 == "Y");
            }
            else if (package == "YARDI5")
            {
                profiles = profiles.Where(x => x.YARDI5 == "Y");
            }
            else if (package == "YARDI4")
            {
                profiles = profiles.Where(x => x.YARDI14 == "Y");
            }
            else if (package == "YARDI7")
            {
                profiles = profiles.Where(x => x.YARDI7 == "Y");
            }
            else if (package == "YARDI8")
            {
                profiles = profiles.Where(x => x.YARDI8 == "Y");
            }
            else if (package == "YARDI11")
            {
                profiles = profiles.Where(x => x.YARDI11 == "Y");
            }

            if(status != string.Empty)
            {
                profiles = profiles.Where(x => x.VendorStatus.ToLower().Trim() == status.ToLower().Trim());
            }

          

            var pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.VendorEIN, i.JDEVendorID, i.VendorStatus, i.VendorAddress1, i.VendorCity, i.VendorState, i.VendorZipCode, i.VendorEmail, i.VendorContactFN,i.VendorContactLN,i.PaymentTerm,i.PersonCorpCode,i.MRI1,i.MRI2,i.MRI10,i.MRI12,i.YARDI5,i.YARDI14,i.YARDI7,i.YARDI8, i.YARDI11, i.LastUpdatedTime }).OrderByDescending(x => x.LastUpdatedTime).ToList();
          //  var pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.JDEVendorID, i.VendorStatus, i.VendorEmail, i.LastUpdatedTime }).OrderByDescending(x => x.LastUpdatedTime);

            var jsonResult = Json(pp, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;

        }


        [HttpGet]
        public async Task<JsonResult> GetVendorList()
        {
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
            // var pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.VendorEIN,i.JDEVendorID,i.VendorStatus,i.VendorAddress1 , i.VendorAddress2, i.VendorCity,i.VendorState,i.VendorZipCode,i.VendorEmail,i.LastUpdatedTime}).OrderByDescending(x=> x.LastUpdatedTime);

            var pp = profiles.Where(y => y.VendorStatus == HelperClass.InviteSent || y.VendorStatus == HelperClass.InAccouting )
                .Select(i => new {
                ID = i.ID,
                VendorDBAName = i.VendorDBAName,
                JDEVendorID = i.JDEVendorID,
                VendorEIN = i.VendorEIN,
                VendorStatus = i.VendorStatus,
                STATUSINJDE = i.STATUSINJDE,
                VendorEmail = i.VendorEmail,
                Address1 = i.VendorAddress1,
                Address2 = i.VendorAddress2,
                VendorState = i.VendorState,
                VendorCity = i.VendorCity,
                VendorZip = i.VendorZipCode,
                VendorPhone = i.VPhone,
                VendorType = HelperClass.ReturnSearchTypeDef(i.VendorType) ,
                VendorLegalName = i.VendorLegalName,
                SubmittedTime = i.SubmittedTime.ToShortDateString() ,
                VPhone = i.VPhone,
                VendorZipCode = i.VendorZipCode,
                AIAddress = i.AIAddress,
                AICity = i.AICity,
                AIState = i.AIState,
                AIZip = i.AIZip,
                LastUpdatedTime =  i.LastUpdatedTime


            }).OrderByDescending(x => x.LastUpdatedTime).ToList();


            var jsonResult = Json(pp, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;


        }


        [HttpGet]
        public async Task<JsonResult> GetAllVendorList()
        {
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
            // var pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.VendorEIN,i.JDEVendorID,i.VendorStatus,i.VendorAddress1 , i.VendorAddress2, i.VendorCity,i.VendorState,i.VendorZipCode,i.VendorEmail,i.LastUpdatedTime}).OrderByDescending(x=> x.LastUpdatedTime);

            var pp = profiles
                .Select(i => new {
                    ID = i.ID,
                    VendorDBAName = i.VendorDBAName,
                    JDEVendorID = i.JDEVendorID,
                    VendorEIN = i.VendorEIN,
                    VendorStatus = i.VendorStatus,
                    STATUSINJDE = i.STATUSINJDE,
                    VendorEmail = i.VendorEmail,
                    Address1 = i.VendorAddress1,
                    Address2 = i.VendorAddress2,
                    VendorState = i.VendorState,
                    VendorCity = i.VendorCity,
                    VendorZip = i.VendorZipCode,
                    VendorPhone = i.VPhone,
                    VendorType = HelperClass.ReturnSearchTypeDef(i.VendorType),
                    VendorLegalName = i.VendorLegalName,
                    SubmittedTime = i.SubmittedTime.ToShortDateString(),
                    VPhone = i.VPhone,
                    VendorZipCode = i.VendorZipCode,
                    AIAddress = i.AIAddress,
                    AICity = i.AICity,
                    AIState = i.AIState,
                    AIZip = i.AIZip,
                    LastUpdatedTime = i.LastUpdatedTime


                }).OrderByDescending(x => x.LastUpdatedTime).ToList();


            var jsonResult = Json(pp, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;


        }

    }
}