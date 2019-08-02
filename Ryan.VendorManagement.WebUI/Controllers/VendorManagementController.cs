using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ryan.VendorManagement.WebUI.Models.vendor;
using Ryan.VendorOnboarding.Domain.Concrete;
using Ryan.VendorManagement.WebUI.Infrastructure.Utilities ;
using Ryan.VendorOnboarding.Domain.Entities;
using System.Threading.Tasks;
using Ryan.VendorOnboarding.Domain;

using Ryan.VendorManagement.WebUI.Infrastructure.Security;
using System.Security.Principal;
using System.Configuration;
using System.IO;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using Ryan.VendorManagement.WebUI.Infrastructure;
using Ryan.EmployeeOst.Domain.Concrete;
using Newtonsoft.Json;
using System.Diagnostics;
using Ryan.EmployeeOst.Domain.Entities;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Web.Hosting;



namespace Ryan.VendorManagement.WebUI.Controllers
{
    public class VendorManagementController : Controller
    {
        EFVendorProfileRepositary vendorProfileRepository = new EFVendorProfileRepositary();
        EFVendorStatusHistory statusHistoryRepo = new EFVendorStatusHistory();
        EFVendorAddressContactsRespositary eFVenAddConsRespo = new EFVendorAddressContactsRespositary();
        EFEmployeeRepository emprepo = new EFEmployeeRepository();
        int pagesize = Convert.ToInt32( ConfigurationManager.AppSettings["pagesizer"].ToString()); // get it from config
        string jdeFilePath = ConfigurationManager.AppSettings["JDEfilepath"].ToString();
        string folderfilepath = ConfigurationManager.AppSettings["filepath"].ToString();
        string documentUrl = ConfigurationManager.AppSettings["documentUrl"].ToString();
        string OracleHostName = ConfigurationManager.AppSettings["OracleHostName"].ToString(); 
        string OraclePort = ConfigurationManager.AppSettings["OraclePort"].ToString();
        string OracleDbName = ConfigurationManager.AppSettings["OracleDbName"].ToString();
        string OraclePwd = ConfigurationManager.AppSettings["OraclePwd"].ToString();
        string OracleUserName = ConfigurationManager.AppSettings["OracleUserName"].ToString();
        string SendEmailUrl = ConfigurationManager.AppSettings["SendEmailUrl"].ToString(); 
        string environment = ConfigurationManager.AppSettings["OracleServerName"].ToString();
        
        EventLog v = new EventLog("Application");
         SecurityCheckResult securityCheckResult = null; 
        public VendorManagementController()
        {
          
           

        }


        //// GET: VendorManagement
        //public ActionResult Index(string SearchText)
        //{
        //    //List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
        //    // IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

        //    //profiles = profiles.OrderByDescending(x => x.LastUpdatedTime).Take(10);
        //    //ViewBag.CurrentFilter = SearchText;

        //    //if (!string.IsNullOrWhiteSpace(SearchText))
        //    //{
        //    // profiles = profiles.Where(x => x.VendorLegalName.Contains(SearchText.ToLower())).Distinct().OrderByDescending(x => x.LastUpdatedTime);
        //    //}

        //    //foreach (VendorProfile pro in profiles)
        //    //{
        //    //    VendorProfileViewModel v = new VendorProfileViewModel();
        //    //    v.VendorLegalName = pro.VendorLegalName;
        //    //    v.VendorEIN = pro.VendorEIN;
        //    //    v.VendorStatus = pro.VendorStatus;
        //    //    v.EmailAddress = pro.VendorEmail;
        //    //    v.ID = pro.ID;
        //    //    v.JDEVendorID = pro.JDEVendorID;
        //    //    v.VendorAddress1 = pro.VendorAddress1;
        //    //    v.VendorAddress2 = pro.VendorAddress2;
        //    //    v.VendorCity = pro.VendorCity;
        //    //    v.VendorState = pro.VendorState;

        //    //    vendorProfiles.Add(v);

        //    //}
        //    ////int num_of_page = (pageNum ?? 1);
        //    //// return View(vendorProfiles.ToPagedList(num_of_page, pagesize));
        //    //return View(vendorProfiles);
        //     return RedirectToAction("AddVendor");
        //  //  return View(); 
        //}

        // GET: VendorManagement
        public async Task<ActionResult> Index()
        {
            // test   users
            // string[] roles = new string[] { @"RYAN\MIN-PROP RBS", @"RYAN\MIN-PROP MGMT MGRS" };

            // test   construction users
            //string[] roles = new string[] { @"RYAN\RYAN-FIELD-ALL" };

            //System.Web.HttpContext.Current.User =
            //   new GenericPrincipal(new GenericIdentity("mmeilstrup"), roles);


            securityCheckResult = await SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser");
            if(!securityCheckResult.Valid)
            {
                return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
            }
            return RedirectToAction("AddVendor");
             
        }

        public ActionResult NoAccess()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> VendorStatus(string SearchText, string SearchBy , DateTime? SubmittedDate , string jdeid)
        {
            try
            {
                List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
                IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorListByNameAndDate(SearchText.ToLower(), SubmittedDate, jdeid);


                List<VendorProfile> prof = new List<VendorProfile>();
                ViewBag.CurrentFilter = SearchText;

                if (!string.IsNullOrEmpty(SearchText))
                {
                    if (SearchBy.ToString() == "true")
                    {
                        // starts with is true
                        profiles = profiles.Where(x => (x.VendorDBAName != null && x.VendorDBAName.ToLower().StartsWith(SearchText.ToLower()))).ToList();

                    }


                    // prof = profiles.ToList();
                }



                foreach (VendorProfile pro in profiles)
                {
                    VendorProfileViewModel v = new VendorProfileViewModel();

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
                    v.JDEStatus = pro.STATUSINJDE;
                    v.VendorEmail = pro.VendorEmail;
                    v.VendorDBAName = pro.VendorDBAName;

                    v.VendorType = HelperClass.ReturnSearchTypeDef(pro.VendorType);
                    v.VendorLegalName = pro.VendorLegalName;
                    v.VPhone = pro.VPhone;
                    v.VendorZipCode = pro.VendorZipCode;
                    v.AIAddress = pro.AIAddress;
                    v.AICity = pro.AICity;
                    v.AIState = pro.AIState;
                    v.AIZip = pro.AIZip;
                    vendorProfiles.Add(v);


                }
                //int num_of_page = (pageNum ?? 1);
                // return View(vendorProfiles.ToPagedList(num_of_page, pagesize));
                return View(vendorProfiles);
            }
            catch(Exception ex)
            {
                return RedirectToAction("VErrorPage");
            }
           
            //return View();
        }

        [HttpGet]
        public ActionResult VendorStatus()
        {
            if (securityCheckResult is null)
            {
                securityCheckResult = SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser").Result;
                if (!securityCheckResult.Valid)
                {
                    return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetVendorList()
        {
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
           // var pp = profiles.Select(i => new { i.ID, i.VendorLegalName, i.VendorDBAName, i.VendorEIN,i.JDEVendorID,i.VendorStatus,i.VendorAddress1 , i.VendorAddress2, i.VendorCity,i.VendorState,i.VendorZipCode,i.VendorEmail,i.LastUpdatedTime}).OrderByDescending(x=> x.LastUpdatedTime);

            var pp = profiles.Select(i => new { i.ID, i.VendorDBAName, i.JDEVendorID, i.VendorStatus, i.VendorEmail,i.VendorType,i.VendorEIN,i.RequestedUserEmail, i.LastUpdatedTime }).Where(x=> x.VendorEIN != null).OrderByDescending(x => x.LastUpdatedTime);


            var jsonResult = Json(pp, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;

            
        }


        public async Task<ActionResult> SearchVendorList(string searchText)
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

            var filted = profiles.Where(x => x.VendorLegalName.Contains(searchText.ToLower()));

            foreach (VendorProfile pro in profiles)
            {
                VendorProfileViewModel v = new VendorProfileViewModel();
                v.VendorLegalName = pro.VendorLegalName;
                v.VendorEIN = pro.VendorEIN;
                v.VendorZipCode = pro.VendorZipCode;
                v.VFax = pro.VFax;
                v.VPhone = pro.VPhone;
                v.VendorDBAName = pro.VendorDBAName;
                v.EmailAddress = pro.CardEmailAddress;
                v.VendorStatus = pro.VendorStatus;
                v.ID = pro.ID;
                vendorProfiles.Add(v);

            }


            return View(vendorProfiles);
        }


        public async Task<ActionResult> VendorDetails(string vid)
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

            VendorProfile pro = profiles.FirstOrDefault(x => x.ID == Convert.ToInt32(vid));
            Session["editvend"] = pro;

            VendorProfileViewModel v = new VendorProfileViewModel
            {
                ID = pro.ID,
                VendorLegalName = pro.VendorLegalName,
                VendorEIN = HelperClass.MaskSsn(pro.VendorEIN) ,
                VendorZipCode = pro.VendorZipCode,
                VFax = pro.VFax,
                VPhone = pro.VPhone,
                VendorDBAName = pro.VendorDBAName,
                PersonCorpCode = pro.PersonCorpCode,
                IsEINValid = pro.IsEinVerified == "Y" ? true : false,
                VendorEmail = pro.VendorEmail,
                VendorStatus = pro.VendorStatus,
                VendorAddress1 = pro.VendorAddress1,
                VendorAddress2 = pro.VendorAddress2,
                VendorState = pro.VendorState,
                VendorCity = pro.VendorCity,
                PersonSubForm = pro.PersonSubForm,
                PersonSubTitle = pro.PersonSubTitle,
                VContactFirstName = pro.VendorContactFN,
                VContactLastName = pro.VendorContactLN,
                AIEmail = pro.AIEmail,
                AIName = pro.AIName,
                AIPhone = pro.AIPhone,
                AIAddress = pro.AIAddress,
                AIAddress2 = pro.AIAddress2,
                AICity = pro.AICity,
                AIState = pro.AIState,
                AIZip = pro.AIZip,
                PaymentTerm = pro.PaymentTerm,
                StatusChangeReason = pro.StatusChangeReason,
                MRI1 = pro.MRI1 == "Y" ? true : false,
                MRI2 = pro.MRI2 == "Y" ? true : false,
                MRI10 = pro.MRI10 == "Y" ? true : false,
                MRI12 = pro.MRI12 == "Y" ? true : false,
                YARDI5 = pro.YARDI5 == "Y" ? true : false,
                YARDI14 = pro.YARDI14 == "Y" ? true : false,
                YARDI7 = pro.YARDI7 == "Y" ? true : false,
                YARDI8 = pro.YARDI8 == "Y" ? true : false,
                YARDI11 = pro.YARDI11 == "Y" ? true : false,
                JDE = pro.JDE == "Y" ? true : false,
                PROCORE = pro.PROCORE == "Y" ? true : false,
                MRI1Code = pro.MRI1Code ,
                MRI12Code = pro.MRI12Code,
                MRI2Code = pro.MRI2Code,
                YARDI5Code = pro.YARDI5Code ,
                YARDI7Code = pro.YARDI7Code,
                YARDI8Code = pro.YARDI8Code,
                YARDI11Code = pro.YARDI11Code,
                YARDI14Code = pro.YARDI14CODE,
                SourceType = pro.SourceType,
                RequestedUserEmail = pro.RequestedUserEmail,
                CardContactName = pro.CardContactName,
                CardContactEmail = pro.CardEmailAddress,
                CardContactPhone = pro.CardContactPhone,
                VendorType =  HelperClass.ReturnSearchTypeDef( pro.VendorType),
                JDEVendorID = pro.JDEVendorID,
                JDEStatus = pro.STATUSINJDE,
                JDEStatuses = HelperClass.GetJDEVendorStatuses(),
                PaymentTerms = HelperClass.GetPayementTerms(),
                Statuses = HelperClass.LoadStatuses(),
                PersonCorpCodes = HelperClass.GetPersonCorpCodes()

            };
            // combine status history
            v.States = HelperClass.LoadStates();

            v.UploadfilesList = GetFilesFromFolder(v.ID.ToString());
            v.StatusHistoryList = await statusHistoryRepo.GetStatusHistoryByID(v.ID);
            Session["VprofVM"] = v;

            return View(v);
        }


      

        [HttpGet]
        public  ActionResult AddVendor()
        {
            if (securityCheckResult is null)
            {
                securityCheckResult = SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser").Result;
                if (!securityCheckResult.Valid)
                {
                    return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
                }
            }



            string userType = SecurityUtility.CheckUserType(User);
            
            if (!HelperClass.UserTypes.Contains(userType))
            {
                return RedirectToAction("NoAccess");
            }
                 

            return View();
        }


        [HttpGet]
        public ActionResult PartialStatusHistoryDetails()
        {
            return PartialStatusHistoryDetails();
        }


        [HttpPost]
        public async Task<ActionResult> ValidateFederalID(VendorProfileViewModel vend)
        {
            string fedid = vend.VendorEIN;
            string vendname = vend.VendorLegalName;
            VendorAddressContacts sta = eFVenAddConsRespo.GetVendorAddressContacts(fedid);
            VendorProfile vp = new VendorProfile();
            vp = (VendorProfile)Session["editvend"];

            if (sta.VendorEIN == fedid && vendname == sta.VendorName)
            {
                vp.IsEinVerified = "Y";
                EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vp);

            }

            return RedirectToAction("EditVendor" , new { vid = vp.ID }); ;
        }


        [HttpGet]
        public async Task<ActionResult> ValidateFederalID(string fedid, string vendname , string vendorid)
        {
            //VendorAddressContacts sta = new VendorAddressContacts();
            //sta = eFVenAddConsRespo.GetVendorAddressContacts(fedid);
            

            if  (!string.IsNullOrEmpty( fedid))
            {
                StatusHistory sh = new StatusHistory();
                VendorProfile vp = new VendorProfile();
                vp = (VendorProfile)Session["editvend"];
                vp.IsEinVerified = "Y";
                vp.VendorEIN = fedid;
                EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vp);

                Session["editvend"] = vp;

                if (saveResult.Errors.Count() > 0)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
                    sh.UpdatedTime = DateTime.Now;
                    sh.VStatus = "";
                    sh.Description = "Federal ID validated by " + sh.UpdatedBy + " on " + sh.UpdatedTime;
                    sh.VID = Convert.ToInt32(vendorid); ;
                    // 
                    EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);
                }

            }
            else {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> AddVendor(VendorManagementViewModel vend )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    v.Source = "Application";

                    string emailuser = SecurityUtility.GetUserEmail(User);   // send email to user who sent this request
                    v.WriteEntry(emailuser, EventLogEntryType.Information, 101, 1);

                    string gu = string.Empty;

                    VendorProfile vprofile = new VendorProfile();
                    Guid vguid = Guid.NewGuid();
                    gu = vguid.ToString();
                    vprofile.VendorDBAName = vend.VendorDBAName;
                    vprofile.VendorLegalName = String.Empty;
                    vprofile.VendorContactFN = vend.VContactFirstName;
                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorGuid = gu;
                    
                    vprofile.STATUSINJDE = HelperClass.NotInJDE;
                    vprofile.RequestedUserEmail = emailuser??"bhargava.iragam@ryancompanies.com";
                    vprofile.SourceType = "RyanInvite";
                    vprofile.VendorType = HelperClass.Vendor;
                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";

                    vprofile.VendorStatus = HelperClass.InviteSent;       
                    vprofile.JDE = vend.JDE ? "Y" : "N";
                    if (SecurityUtility.CheckUserRole(User) == "ConstructionUser")
                    {
                        vprofile.PROCORE = "Y";
                    }
                    else
                    {
                        vprofile.PROCORE = vend.PROCORE ? "Y" : "N";
                    }

                    vprofile.SubmittedTime = DateTime.Now;
               
                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    string loc = Server.MapPath("~/Views/EmailTemplates/InviteVendorTemplate.html");
                    using (StreamReader reader = new StreamReader(loc))
                    {
                        body = reader.ReadToEnd();
                    }

                    // special instrcuctions
                    body = body.Replace("{SpecialInstructions}", vend.SpecialInstructions);

                    body = body.Replace("{SendEmailUrl}", SendEmailUrl); //replacing the required things  
                    body = body.Replace("{email}", vprofile.VendorGuid  ); //replacing the required things  

                    // Email to Client
                    List<string> vemails = new List<string>();

                    //  emails.Add(vend.VendorEmail);

                    vemails.Add(vend.VendorEmail);

                    //if (string.IsNullOrEmpty(emailuser))
                    //{
                    //    emails.Add(emailuser);
                    //}

                    //string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                    //string[] Aemails = AdminEmails.Split(',');
                    //foreach (string s in Aemails)
                    //{
                    //    emails.Add(s);
                    //}

                    string Subject = ConfigurationManager.AppSettings["emailsub1"].ToString(); 

                    // Save here
                 if(!string.IsNullOrEmpty(vend.ID))
                    {
                        IEnumerable<VendorProfile> v = await vendorProfileRepository.GetVendorProfiles();
                        VendorProfile vp = v.Where(x => x.ID == Convert.ToInt32(vend.ID)).FirstOrDefault();
                        if( vp != null  && (vp.VendorStatus == HelperClass.InviteSent))
                        {
                            vp.VendorDBAName = vend.VendorDBAName;
                            vp.VendorContactFN = vend.VContactFirstName;
                            vp.VendorContactLN = vend.VContactLastName;
                            vp.VendorEmail = vend.VendorEmail;

                            if (string.IsNullOrEmpty(vp.VendorGuid))
                            {
                                vp.VendorGuid = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                gu = vp.VendorGuid;
                            }

                            vp.STATUSINJDE = HelperClass.NotInJDE;
                            vp.RequestedUserEmail = emailuser ?? "";
                            vp.SourceType = "RyanInvite";
                            vp.VendorType = HelperClass.Vendor;
                            vp.MRI1 = vend.MRI1 ? "Y" : "N";
                            vp.MRI2 = vend.MRI2 ? "Y" : "N";
                            vp.MRI10 = vend.MRI10 ? "Y" : "N";
                            vp.MRI12 = vend.MRI12 ? "Y" : "N";
                            vp.YARDI5 = vend.YARDI5 ? "Y" : "N";
                            vp.YARDI14 = vend.YARDI14 ? "Y" : "N";
                            vp.YARDI7 = vend.YARDI7 ? "Y" : "N";
                            vp.YARDI8 = vend.YARDI8 ? "Y" : "N";
                            vp.YARDI11 = vend.YARDI11 ? "Y" : "N";
                            vp.JDE = vend.JDE ? "Y" : "N";
                            vp.PROCORE = vend.PROCORE ? "Y" : "N";
                            vp.SubmittedTime = DateTime.Now;

                            EFDbActionResult saveResult1 = await vendorProfileRepository.SaveVendorDetials(vp);
                            if (saveResult1.Succeeded)
                            {
                                string des1 = "Re Invite request sent to vendor again on " + DateTime.Now.ToShortDateString() + " by " + SecurityUtility.GetSimpleUserName(User);
                                await HelperClass.SaveStatuHistoryRecord(des1, HelperClass.InviteSent, saveResult1.SavedID);
                                TempData["AlertMessage"] = "Vendor Re-Invite request successful.";
                                return RedirectToAction("AddVendor");

                            }
                            else
                            {
                                TempData["AlertMessage"] = "There was an error saving the vendor, please try again.";
                                return View(vend);
                            }
                          
                        }
                        //else
                        //{
                        //    EFDbActionResult saveResult1 = await vendorProfileRepository.SaveVendorDetials(vp);
                        //    vp.VendorGuid = Guid.NewGuid().ToString();
                        //    TempData["AlertMessage"] = "Vendor Re-Invite request successful.";

                        //}
                    }
                 //else
                 //   {
                        EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);
                        v.WriteEntry("Model saved successfully", EventLogEntryType.Information, 101, 1);

                        // update status history table
                        string des = "Invite request sent to vendor on " + DateTime.Now.ToShortDateString() + " by " + SecurityUtility.GetSimpleUserName(User);

                        await HelperClass.SaveStatuHistoryRecord(des, HelperClass.InviteSent, saveResult.SavedID);

                        TempData["AlertMessage"] = "Vendor Invite request successful.";

                    //}




                    await EmailNotificationUtility.SendEmailToClient(vemails, gu, vend.VendorDBAName,body,Subject);
                    return RedirectToAction("AddVendor");
                }
                else
                {
                    TempData["AlertMessage"] = "There was an error saving the vendor, please try again.";
                    return View(vend);
                }

            }
            catch(Exception ex)
            {
                v.WriteEntry(ex.Message + " ---- " + ex.StackTrace, EventLogEntryType.Information, 101, 1);
                TempData["AlertMessage"] = "There was an error saving the vendor, please try again.";
                return View(); ;

            }
           
        }

        private async Task SendEmailToClient(string vemail, string vendorguid, string VendorDBAName)
        {
        
            string SmtpServer = "Relay.ryancompanies.com";
            

           
            string body = string.Format("Hi {0} Vendor , Please click below link and submit your details to Ryan companies.: <a href='http://ryweb16-d:5895/Home/Index?ID={1}' target='_blank'>Submit Details</a>. If you need assistance, please email <a href = 'mailto: vendor.mgmt@ryancompanies.com'>VendorManagementSupport</a> . ", VendorDBAName, vendorguid);
            string Subject = string.Format(" {0} , welcome to ryan.", VendorDBAName);
            


            var message = new MailMessage();
            message.To.Add(vemail);
            message.From = new MailAddress("Vendor.mgmt@RyanCompanies.com"); 
            message.Subject = Subject;
            message.Body = body;

            using (var smtpClient = new SmtpClient(SmtpServer))
            {
                await smtpClient.SendMailAsync(message);
            }
        }


      



        /// <summary>
        /// AUto complete process while adding vendor
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetStatusHistoryFromID(string id)
        {
            var statusHist = await statusHistoryRepo.GetStatusHistoryDetailsByID(Convert.ToInt32(id));
           

            return Json(statusHist, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// AUto complete process while adding vendor
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult AutoComplete(string prefix)
        {
            try
            {
                var customers = vendorProfileRepository.GetAllVendorDetails().Where(x => x.VendorDBAName != null && x.VendorDBAName.ToLower().Contains(prefix.ToLower())  && (x.VendorStatus != "Cancelled")).ToList();
                IEnumerable<VendorProfile> prf = customers.Take(10);


                return Json(prf, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
            }

            return Json( null, JsonRequestBehavior.AllowGet);

        }



        
        public JsonResult ViewDocument(string name)
        {
            var customers = vendorProfileRepository.GetAllVendorDetails().Where(x => x.VendorDBAName != null);
            IEnumerable<VendorProfile> prf = customers.Where(x => x.VendorDBAName.ToLower().Contains(name.ToLower())).Take(10);


            return Json(prf, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> EditNonPaidVendor(string vid)
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
            VendorProfile pro = profiles.FirstOrDefault(x => x.ID == Convert.ToInt32(vid));

            if (pro is null)
            {
                return RedirectToAction("VErrorPage");
            }
            // Get status history list
            IEnumerable<StatusHistory> statusHistList = await statusHistoryRepo.GetStatusHistoryByID(Convert.ToInt32(vid));

            Session["statushistlist"] = statusHistList.Take(5);



            Session["editvend"] = pro;

            VendorProfileViewModel v = new VendorProfileViewModel
            {
                ID = pro.ID,
                VendorLegalName = pro.VendorLegalName,
                VendorEIN = pro.VendorEIN,
                VendorZipCode = pro.VendorZipCode,
                VFax = pro.VFax,
                VPhone = pro.VPhone,
                VendorDBAName = pro.VendorDBAName,
                PersonCorpCode = pro.PersonCorpCode ?? "",
                IsEINValid = pro.IsEinVerified == "Y" ? true : false,
                VendorEmail = pro.VendorEmail,
                VendorStatus = pro.VendorStatus ?? "",
                VendorAddress1 = pro.VendorAddress1,
                VendorAddress2 = pro.VendorAddress2,
                VendorState = pro.VendorState ?? "",
                VendorCity = pro.VendorCity,
                PersonSubForm = pro.PersonSubForm,
                PersonSubTitle = pro.PersonSubTitle,
                VContactFirstName = pro.VendorContactFN,
                VContactLastName = pro.VendorContactLN,
                AIEmail = pro.AIEmail,
                AIName = pro.AIName,
                AIPhone = pro.AIPhone,
                AIAddress = pro.AIAddress,
                AIAddress2 = pro.AIAddress2,
                AICity = pro.AICity,
                AIState = pro.AIState ?? "",
                AIZip = pro.AIZip,
                PaymentTerm = pro.PaymentTerm ?? "",
                StatusChangeReason = pro.StatusChangeReason,
                MRI1 = pro.MRI1 == "Y" ? true : false,
                MRI2 = pro.MRI2 == "Y" ? true : false,
                MRI10 = pro.MRI10 == "Y" ? true : false,
                MRI12 = pro.MRI12 == "Y" ? true : false,
                YARDI5 = pro.YARDI5 == "Y" ? true : false,
                YARDI14 = pro.YARDI14 == "Y" ? true : false,
                YARDI7 = pro.YARDI7 == "Y" ? true : false,
                YARDI8 = pro.YARDI8 == "Y" ? true : false,
                YARDI11 = pro.YARDI11 == "Y" ? true : false,
                JDE = pro.JDE == "Y" ? true : false,
                PROCORE = pro.PROCORE == "Y" ? true : false,
                VendorType = pro.VendorType,
                JDEVendorID = pro.JDEVendorID,
                JDEStatus = pro.STATUSINJDE ?? "",
                MRI1Code = pro.MRI1Code,
                MRI2Code = pro.MRI2Code,
                MRI12Code = pro.MRI12Code,
                YARDI5Code = pro.YARDI5Code,
                YARDI7Code = pro.YARDI7Code,
                YARDI11Code = pro.YARDI11Code,
                YARDI8Code = pro.YARDI8Code,
                RequestedUserEmail = pro.RequestedUserEmail,
                JDEStatuses = HelperClass.GetJDEVendorStatuses(),
                PaymentTerms = HelperClass.GetPayementTerms(),
                Statuses = HelperClass.LoadStatuses(),
                PersonCorpCodes = HelperClass.GetPersonCorpCodes()

            };
            // combine status history
            v.StatusHistoryList = statusHistList;
            v.States = HelperClass.LoadStates();
            // v.PaymentTerms = GetPayementTerms();

            v.UploadfilesList = GetFilesFromFolder(v.ID.ToString());

            Session["VprofVM"] = v;

            return View(v);
        }

        [HttpGet]
        public async Task<ActionResult> EditVendor(string vid)
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();
            VendorProfile pro = profiles.FirstOrDefault(x => x.ID == Convert.ToInt32(vid));

            if(pro is null)
            {
                return RedirectToAction("VErrorPage");
            }
            // Get status history list
            IEnumerable<StatusHistory> statusHistList = await statusHistoryRepo.GetStatusHistoryByID(Convert.ToInt32(vid));

            Session["statushistlist"] = statusHistList.Take(5);

           

            Session["editvend"] = pro;

            VendorProfileViewModel v = new VendorProfileViewModel
            {
                ID = pro.ID,
                VendorLegalName = pro.VendorLegalName,
                VendorEIN = pro.VendorEIN,
                VendorZipCode = pro.VendorZipCode,
                VFax = pro.VFax,
                VPhone = pro.VPhone,
                VendorDBAName = pro.VendorDBAName,
                PersonCorpCode = pro.PersonCorpCode ?? "",
                IsEINValid = pro.IsEinVerified == "Y" ? true : false,
                VendorEmail = pro.VendorEmail,
                VendorStatus = pro.VendorStatus?? "",
                VendorAddress1 = pro.VendorAddress1,
                VendorAddress2 = pro.VendorAddress2,
                VendorState = pro.VendorState??"",
                VendorCity = pro.VendorCity,
                PersonSubForm = pro.PersonSubForm,
                PersonSubTitle = pro.PersonSubTitle,
                VContactFirstName = pro.VendorContactFN,
                VContactLastName = pro.VendorContactLN,
                AIEmail = pro.AIEmail,
                AIName = pro.AIName,
                AIPhone = pro.AIPhone,
                AIAddress = pro.AIAddress,
                AIAddress2 = pro.AIAddress2,
                AICity = pro.AICity,
                AIState = pro.AIState ?? "",
                AIZip = pro.AIZip,
                PaymentTerm = pro.PaymentTerm ?? "",
                StatusChangeReason = pro.StatusChangeReason,
                MRI1 = pro.MRI1 == "Y" ? true : false,
                MRI2 = pro.MRI2 == "Y" ? true : false,
                MRI10 = pro.MRI10 == "Y" ? true : false,
                MRI12 = pro.MRI12 == "Y" ? true : false,
                YARDI5 = pro.YARDI5 == "Y" ? true : false,
                YARDI14 = pro.YARDI14 == "Y" ? true : false,
                YARDI7 = pro.YARDI7 == "Y" ? true : false,
                YARDI8 = pro.YARDI8 == "Y" ? true : false,
                YARDI11 = pro.YARDI11 == "Y" ? true : false,
                JDE = pro.JDE == "Y" ? true : false,
                PROCORE = pro.PROCORE == "Y" ? true : false,
                VendorType = pro.VendorType,
               JDEVendorID = pro.JDEVendorID,
               JDEStatus = pro.STATUSINJDE??"",
               MRI1Code = pro.MRI1Code,
               MRI2Code = pro.MRI2Code,
               MRI12Code = pro.MRI12Code,
               YARDI5Code = pro.YARDI5Code,
               YARDI7Code = pro.YARDI7Code,
               YARDI11Code = pro.YARDI11Code,
                YARDI14Code = pro.YARDI14CODE,
                YARDI8Code = pro.YARDI8Code,
               CardContactName = pro.CardContactName,
               CardContactEmail = pro.CardEmailAddress,
               CardContactPhone = pro.CardContactPhone,
               RequestedUserEmail = pro.RequestedUserEmail,
               SourceType = pro.SourceType,
               JDEStatuses = HelperClass.GetJDEVendorStatuses(),
                PaymentTerms = HelperClass.GetPayementTerms(),
                Statuses = HelperClass.LoadStatuses(),
                PersonCorpCodes = HelperClass.GetPersonCorpCodes()
            
            };
            // combine status history
            v.StatusHistoryList = statusHistList;
            v.States = HelperClass.LoadStates();
           // v.PaymentTerms = GetPayementTerms();

            v.UploadfilesList = GetFilesFromFolder(v.ID.ToString());

            Session["VprofVM"] = v;

            return View(v);
        }

        [HttpPost]
        public async Task<ActionResult> EditNonPaidVendor(VendorProfileViewModel vend, string button, IEnumerable<HttpPostedFileBase> upload)
        {
            VendorProfile vprof = Session["editvend"] as VendorProfile;
            VendorProfileViewModel vprofVM = Session["VprofVM"] as VendorProfileViewModel;

            StringBuilder statuschange = new StringBuilder();
            bool vendorChanged = false;

            if (ModelState.IsValid)
            {
                try
                {
                    if (Session["editvend"] == null)
                    {
                        TempData["AlertMessage"] = "Vendor does not exist.";
                        return RedirectToAction("VErrorPage");
                    }

                    if (button.ToString().Trim() == "Resend Invite")
                    {
                        TempData["AlertMessage"] = "Vendor update successful.";

                    }

                    // IEnumerable<string> ct = ChangedFields<VendorProfileViewModel>(vprofVM, vend);

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorLegalName) != HelperClass.GetNUllorEmptystring(vend.VendorLegalName))
                    {
                        vendorChanged = true;
                        statuschange.Append("Phone : " + vprofVM.VendorLegalName + " Changed to " + vend.VendorLegalName + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorState) != HelperClass.GetNUllorEmptystring(vend.VendorState))
                    {
                        vendorChanged = true;
                        statuschange.Append("State : " + vprofVM.VendorState + " Changed to " + vend.VendorState + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorStatus) != HelperClass.GetNUllorEmptystring(vend.VendorStatus))
                    {
                        vendorChanged = true;
                        statuschange.Append("Status : " + vprofVM.VendorStatus + " Changed to " + vend.VendorStatus + ". " + System.Environment.NewLine);

                        List<string> emails = new List<string>();
                        string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                        string[] Aemails = AdminEmails.Split(',');

                        if (vend.VendorStatus == HelperClass.Cancelled)
                        {
                            emails.Add("Mitch.Meilstrup@ryancompanies.com");
                            emails.Add("bhargava.iragam@ryancompanies.com");
                            string Subject = vend.VendorDBAName + " is changed to cancelled status.";
                            string body = vend.VendorDBAName + " is changed to cancelled status in vendor onboarding system.";
                            await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                        }


                        if (vend.VendorStatus == "OnHold")
                        {
                            emails.Add("Mitch.Meilstrup@ryancompanies.com");
                            emails.Add("bhargava.iragam@ryancompanies.com");
                            string Subject = vend.VendorDBAName + " is changed to OnHold status.";
                            string body = vend.VendorDBAName + " is changed to OnHold status in vendor onboarding system.";
                            await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                        }

                        if (vend.VendorStatus.ToLower() == HelperClass.Approved.ToLower())
                        {
                            foreach (string s in Aemails)
                            {
                                emails.Add(s);
                            }
                            //if (!string.IsNullOrEmpty(vprof.RequestedUserEmail))
                            //{
                            //    emails.Add(vprof.RequestedUserEmail);
                            //}
                            string Subject = vend.VendorDBAName + " is changed to Approved status.";
                            string body = vend.VendorDBAName + " is changed to Approved status in vendor onboarding system.";
                            await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                        }

                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorZipCode) != HelperClass.GetNUllorEmptystring(vend.VendorZipCode))
                    {
                        vendorChanged = true;
                        statuschange.Append("ZipCode : " + vprofVM.VendorZipCode + " Changed to " + vend.VendorZipCode + ". " + System.Environment.NewLine);
                    }


                    if (HelperClass.GetNUllorEmptystring(vprofVM.VPhone) != HelperClass.GetNUllorEmptystring(vend.VPhone))
                    {
                        vendorChanged = true;
                        statuschange.Append("Phone : " + vprofVM.VPhone + " Changed to " + vend.VPhone + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VFax) != HelperClass.GetNUllorEmptystring(vend.VFax))
                    {
                        vendorChanged = true;
                        statuschange.Append("FAX : " + vprofVM.VFax + " Changed to " + vend.VFax + ".  " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorEIN) != HelperClass.GetNUllorEmptystring(vend.VendorEIN))
                    {
                        vendorChanged = true;
                        statuschange.Append("FederalID : " + vprofVM.VendorEIN + " Changed to " + vend.VendorEIN + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorDBAName) != HelperClass.GetNUllorEmptystring(vend.VendorDBAName))
                    {
                        vendorChanged = true;
                        statuschange.Append("DBA Name" + vprofVM.VendorDBAName + " Changed to " + vend.VendorDBAName + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VContactFirstName) != HelperClass.GetNUllorEmptystring(vend.VContactFirstName))
                    {
                        vendorChanged = true;
                        statuschange.Append("Contact FirstName" + vprofVM.VContactFirstName + " Changed to " + vend.VContactFirstName + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VContactLastName) != HelperClass.GetNUllorEmptystring(vend.VContactLastName))
                    {
                        vendorChanged = true;
                        statuschange.Append("Contact LastName" + vprofVM.VContactLastName + " Changed to " + vend.VContactLastName + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorAddress1) != HelperClass.GetNUllorEmptystring(vend.VendorAddress1))
                    {
                        vendorChanged = true;
                        statuschange.Append("Address1 : " + vprofVM.VendorAddress1 + " Changed to " + vend.VendorAddress1 + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorAddress2) != HelperClass.GetNUllorEmptystring(vend.VendorAddress2))
                    {
                        vendorChanged = true;
                        statuschange.Append("Address2 : " + vprofVM.VendorAddress2 + " Changed to " + vend.VendorAddress2 + ". " + System.Environment.NewLine);
                    }


                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorCity) != HelperClass.GetNUllorEmptystring(vend.VendorCity))
                    {
                        vendorChanged = true;
                        statuschange.Append("City : " + vprofVM.VendorCity + " Changed to " + vend.VendorCity + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.VendorEmail) != HelperClass.GetNUllorEmptystring(vend.VendorEmail))
                    {
                        vendorChanged = true;
                        statuschange.Append("Email : " + vprofVM.VendorEmail + " Changed to " + vend.VendorEmail + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.AIAddress) != HelperClass.GetNUllorEmptystring(vend.AIAddress))
                    {
                        vendorChanged = true;
                        statuschange.Append("AIAddress : " + vprofVM.AIAddress + " Changed to " + vend.AIAddress + ". " + System.Environment.NewLine);
                    }

                    if (HelperClass.GetNUllorEmptystring(vprofVM.AIAddress2) != HelperClass.GetNUllorEmptystring(vend.AIAddress2))
                    {
                        vendorChanged = true;
                        statuschange.Append("AIAddress2 :" + vprofVM.AIAddress2 + " Changed to " + vend.AIAddress2 + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.AICity) != HelperClass.GetNUllorEmptystring(vend.AICity))
                    {
                        vendorChanged = true;
                        statuschange.Append("AICity :" + vprofVM.AICity + " Changed to " + vend.AICity + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.AIState) != HelperClass.GetNUllorEmptystring(vend.AIState))
                    {
                        vendorChanged = true;
                        statuschange.Append("AIState :" + vprofVM.AIState + " Changed to " + vend.AIState + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.AIZip) != HelperClass.GetNUllorEmptystring(vend.AIZip))
                    {
                        vendorChanged = true;
                        statuschange.Append("AIZip :" + vprofVM.AIZip + " Changed to " + vend.AIZip + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.PaymentTerm) != HelperClass.GetNUllorEmptystring(vend.PaymentTerm))
                    {
                        vendorChanged = true;
                        statuschange.Append("PaymentTerm :" + vprofVM.PaymentTerm + " Changed to " + vend.PaymentTerm + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.PersonCorpCode) != HelperClass.GetNUllorEmptystring(vend.PersonCorpCode))
                    {
                        vendorChanged = true;
                        statuschange.Append("PersonCorpCode :" + vprofVM.PersonCorpCode + " Changed to " + vend.PersonCorpCode + ". " + System.Environment.NewLine);
                    }
                    if (HelperClass.GetNUllorEmptystring(vprofVM.PersonSubTitle) != HelperClass.GetNUllorEmptystring(vend.PersonSubTitle))
                    {
                        vendorChanged = true;
                        statuschange.Append("PersonSubTitle :" + vprofVM.PersonSubTitle + " Changed to " + vend.PersonSubTitle + ". " + System.Environment.NewLine);
                    }

                    StatusHistory sh = new StatusHistory();
                    VendorProfile vprofile = new VendorProfile();

                    v.Source = "Application";
                    // get it from session
                    vprofile.ID = vprof.ID;
                    vprofile.JDEVendorID = vprof.JDEVendorID;
                    vprofile.VRVENDORNUMBER = vprof.VRVENDORNUMBER;
                    vprofile.VendorGuid = vprof.VendorGuid;
                    vprofile.IsEinVerified = vprof.IsEinVerified;
                    vprofile.RequestedUserEmail = vprof.RequestedUserEmail;
                    vprofile.SubmittedTime = vprof.SubmittedTime;
                    // get from VM
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorLegalName = string.IsNullOrEmpty(vend.VendorLegalName) ? vend.VendorDBAName : vend.VendorLegalName;
                    vprofile.VendorStatus = vend.VendorStatus;

                    vprofile.VendorDBAName = vend.VendorDBAName;
                    if (vprof.IsEinVerified == "Y")
                    {
                        vprofile.VendorEIN = vprof.VendorEIN;
                    }
                    else
                    {
                        vprofile.VendorEIN = vend.VendorEIN;
                    }

                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorContactFN = vend.VContactFirstName;
                    // checking value
                    v.WriteEntry("*** " + vprofile.VendorEIN, EventLogEntryType.Information, 101, 1);

                    vprofile.VPhone = vend.VPhone;
                    vprofile.VFax = vend.VFax;
                    vprofile.VendorCity = vend.VendorCity;
                    vprofile.VendorState = vend.VendorState ?? "";
                    vprofile.VendorAddress1 = vend.VendorAddress1;
                    vprofile.VendorAddress2 = vend.VendorAddress2;
                    vprofile.VendorZipCode = vend.VendorZipCode;
                    vprofile.PersonCorpCode = vend.PersonCorpCode;
                    vprofile.PersonSubForm = vend.PersonSubForm;
                    vprofile.PersonSubTitle = vend.PersonSubTitle;
                    vprofile.UpdatedBy = SecurityUtility.GetSimpleUserName(User);
                    vprofile.LastUpdatedTime = DateTime.Now;
                    vprofile.AIName = vend.AIName;
                    vprofile.AIEmail = vend.AIEmail;
                    vprofile.AIPhone = vend.AIPhone;
                    vprofile.AIState = vend.AIState ?? "";
                    vprofile.AICity = vend.AICity;
                    vprofile.AIAddress = vend.AIAddress;
                    vprofile.AIAddress2 = vend.AIAddress2;
                    vprofile.AIZip = vend.AIZip;
                    vprofile.PaymentTerm = vend.PaymentTerm ?? "";
                    vprofile.StatusChangeReason = vend.StatusChangeReason;
                    vprofile.STATUSINJDE = vend.JDEStatus ?? "";
                    vprofile.VendorType = vend.VendorType;

                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";
                    vprofile.JDE = vend.JDE ? "Y" : "N";
                    vprofile.PROCORE = vend.PROCORE ? "Y" : "N";

                    vprofile.MRI1Code = vend.MRI1Code;
                    vprofile.MRI2Code = vend.MRI2Code;
                    vprofile.MRI12Code = vend.MRI12Code;
                    vprofile.YARDI5Code = vend.YARDI5Code;
                    vprofile.YARDI7Code = vend.YARDI7Code;
                    vprofile.YARDI8Code = vend.YARDI8Code;
                    vprofile.YARDI11Code = vend.YARDI11Code;


                    // save 
                    if (button.ToString().Trim() == "Save")
                    {
                        string filepath1 = ConfigurationManager.AppSettings["filepath"].ToString();
                        string filepath = filepath1 + vprof.ID.ToString() + "//";

                       

                        if (upload != null)
                        {
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }
                            // v.WriteEntry("Saving to " + path + vprof.ID.ToString(), EventLogEntryType.Information, 101, 1);
                            foreach (var file in upload)
                            {
                                string sv = filepath1 + vprof.ID.ToString() + "\\" + System.IO.Path.GetFileName(file.FileName);
                                file.SaveAs(sv);
                            }
                        }


                        IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);
                        TempData["AlertMessage"] = "Vendor update successful.";
                        v.WriteEntry("*** succesfully save", EventLogEntryType.Information, 101, 1);
                    }

                    if (vendorChanged)
                    {
                        sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
                        sh.UpdatedTime = DateTime.Now;
                        sh.VStatus = vend.VendorStatus;
                        sh.Description = statuschange.ToString();
                        sh.VID = vprof.ID;
                        // 
                        EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);
                    }

                    if (button == "ExportJDE")
                    {
                        vprofile.JDEVendorID = await ExportToJDE(vprofile);
                        TempData["AlertMessage"] = "Vendor Data has been moved to JDE. Please give one day to transfer.";
                    }
                }
                catch (Exception e)
                {
                    TempData["AlertMessage"] = "There was an error saving the vendor profile, please try again.";

                    v.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
                    return RedirectToAction("VErrorPage");
                }
                // send email if send email has been pressed
                return RedirectToAction("VendorStatus", "Admin");
            }
            else
            {

                TempData["AlertMessage"] = "Please fill all the details and please try again.";
                vend.Statuses = HelperClass.LoadStatuses();
                vend.States = HelperClass.LoadStates();
                vend.PaymentTerms = HelperClass.GetPayementTerms();
                vend.JDEStatuses = HelperClass.GetJDEVendorStatuses();
                IEnumerable<StatusHistory> statusHistList = Session["statushistlist"] as IEnumerable<StatusHistory>;
                vend.StatusHistoryList = statusHistList;
                vend.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
                vend.UploadfilesList = GetFilesFromFolder(vprof.ID.ToString());
                return View(vend);
            }

        }

        [HttpPost]
        public async Task<ActionResult> EditVendor(VendorProfileViewModel vend, string button, IEnumerable<HttpPostedFileBase> upload)
        {
            VendorProfile vprof = Session["editvend"] as VendorProfile;
         VendorProfileViewModel vprofVM = Session["VprofVM"] as VendorProfileViewModel;
            string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
            string[] Aemails = AdminEmails.Split(',');

            StringBuilder statuschange = new StringBuilder();
            bool vendorChanged = false;

            if (ModelState.IsValid)
            {
                try
                {
                    if(Session["editvend"] == null)
                    {
                        TempData["AlertMessage"] = "Vendor does not exist.";
                        return RedirectToAction("VErrorPage");
                    }

                    if (button.ToString().Trim() == "Resend Invite")
                    {
                        TempData["AlertMessage"] = "Vendor update successful.";
                       
                    }

                    // IEnumerable<string> ct = ChangedFields<VendorProfileViewModel>(vprofVM, vend);

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorLegalName) != HelperClass.GetNUllorEmptystring( vend.VendorLegalName))
                        {
                            vendorChanged = true;
                            statuschange.Append("Phone : " + vprofVM.VendorLegalName + " Changed to " + vend.VendorLegalName + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorState) != HelperClass.GetNUllorEmptystring(vend.VendorState))
                        {
                            vendorChanged = true;
                            statuschange.Append("State : " + vprofVM.VendorState + " Changed to " + vend.VendorState + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorStatus) != HelperClass.GetNUllorEmptystring(vend.VendorStatus))
                        {
                            vendorChanged = true;
                            statuschange.Append("Status : " + vprofVM.VendorStatus + " Changed to " + vend.VendorStatus + ". " + System.Environment.NewLine);

                            List<string> emails = new List<string>();
                           
                            

                            if (vend.VendorStatus ==  HelperClass.Cancelled )
                            {
                                emails.Add("Mitch.Meilstrup@ryancompanies.com");
                                emails.Add("bhargava.iragam@ryancompanies.com");
                                string Subject =  vend.VendorDBAName + " is changed to cancelled status.";
                                string body = vend.VendorDBAName + " is changed to cancelled status in vendor onboarding system.";
                                await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                            }


                            if (vend.VendorStatus == "OnHold")
                            {
                                emails.Add("Mitch.Meilstrup@ryancompanies.com");
                                emails.Add("bhargava.iragam@ryancompanies.com");
                                string Subject = vend.VendorDBAName + " is changed to OnHold status.";
                                string body = vend.VendorDBAName + " is changed to OnHold status in vendor onboarding system.";
                                await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                            }

                            if (vend.VendorStatus.ToLower() == HelperClass.Approved.ToLower() )
                            {
                                foreach (string s in Aemails)
                                {
                                    emails.Add(s);
                                }
                                if (string.IsNullOrEmpty(vprof.JDEVendorID))
                                {
                                    if (!string.IsNullOrEmpty(vprof.RequestedUserEmail))
                                    {
                                        emails.Add(vprof.RequestedUserEmail);
                                    }
                                }
                            string Subject = vend.VendorDBAName + " is changed to Approved status.";
                                string body = vend.VendorDBAName + " is changed to Approved status in vendor onboarding system.";
                                await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, body, Subject);
                            }

                    }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorZipCode)!= HelperClass.GetNUllorEmptystring(vend.VendorZipCode))
                        {
                            vendorChanged = true;
                            statuschange.Append("ZipCode : " + vprofVM.VendorZipCode + " Changed to " + vend.VendorZipCode + ". " + System.Environment.NewLine);
                        }
                      

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VPhone) != HelperClass.GetNUllorEmptystring(vend.VPhone))
                        {
                            vendorChanged = true;
                            statuschange.Append("Phone : " + vprofVM.VPhone + " Changed to " + vend.VPhone + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VFax) != HelperClass.GetNUllorEmptystring(vend.VFax))
                        {
                            vendorChanged = true;
                            statuschange.Append("FAX : " + vprofVM.VFax + " Changed to " + vend.VFax + ".  " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorEIN) != HelperClass.GetNUllorEmptystring(vend.VendorEIN))
                        {
                            vendorChanged = true;
                            statuschange.Append("FederalID : " + vprofVM.VendorEIN + " Changed to " + vend.VendorEIN + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorDBAName) != HelperClass.GetNUllorEmptystring(vend.VendorDBAName))
                        {
                            vendorChanged = true;
                            statuschange.Append("DBA Name" + vprofVM.VendorDBAName + " Changed to " + vend.VendorDBAName + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VContactFirstName) != HelperClass.GetNUllorEmptystring(vend.VContactFirstName))
                        {
                            vendorChanged = true;
                            statuschange.Append("Contact FirstName" + vprofVM.VContactFirstName + " Changed to " + vend.VContactFirstName + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VContactLastName) != HelperClass.GetNUllorEmptystring(vend.VContactLastName))
                        {
                            vendorChanged = true;
                            statuschange.Append("Contact LastName" + vprofVM.VContactLastName + " Changed to " + vend.VContactLastName + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring( vprofVM.VendorAddress1) != HelperClass.GetNUllorEmptystring(vend.VendorAddress1))
                        {
                            vendorChanged = true;
                            statuschange.Append("Address1 : " + vprofVM.VendorAddress1 + " Changed to " + vend.VendorAddress1 + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorAddress2) != HelperClass.GetNUllorEmptystring(vend.VendorAddress2))
                        {
                            vendorChanged = true;
                            statuschange.Append("Address2 : " + vprofVM.VendorAddress2 + " Changed to " + vend.VendorAddress2 + ". " + System.Environment.NewLine);
                        }


                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorCity) != HelperClass.GetNUllorEmptystring(vend.VendorCity))
                        {
                            vendorChanged = true;
                            statuschange.Append("City : " + vprofVM.VendorCity + " Changed to " + vend.VendorCity + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.VendorEmail) != HelperClass.GetNUllorEmptystring(vend.VendorEmail))
                        {
                            vendorChanged = true;
                            statuschange.Append("Email : " + vprofVM.VendorEmail + " Changed to " + vend.VendorEmail + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.AIAddress) != HelperClass.GetNUllorEmptystring(vend.AIAddress))
                        {
                            vendorChanged = true;
                            statuschange.Append("AIAddress : " + vprofVM.AIAddress + " Changed to " + vend.AIAddress + ". " + System.Environment.NewLine);
                        }

                        if (HelperClass.GetNUllorEmptystring(vprofVM.AIAddress2) != HelperClass.GetNUllorEmptystring(vend.AIAddress2))
                        {
                            vendorChanged = true;
                            statuschange.Append("AIAddress2 :" + vprofVM.AIAddress2 + " Changed to " + vend.AIAddress2 + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.AICity) != HelperClass.GetNUllorEmptystring(vend.AICity))
                        {
                            vendorChanged = true;
                            statuschange.Append("AICity :" + vprofVM.AICity + " Changed to " + vend.AICity + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.AIState) != HelperClass.GetNUllorEmptystring(vend.AIState))
                        {
                            vendorChanged = true;
                            statuschange.Append("AIState :" + vprofVM.AIState + " Changed to " + vend.AIState + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.AIZip) != HelperClass.GetNUllorEmptystring(vend.AIZip))
                        {
                            vendorChanged = true;
                            statuschange.Append("AIZip :" + vprofVM.AIZip + " Changed to " + vend.AIZip + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.PaymentTerm) != HelperClass.GetNUllorEmptystring(vend.PaymentTerm))
                        {
                            vendorChanged = true;
                            statuschange.Append("PaymentTerm :" + vprofVM.PaymentTerm + " Changed to " + vend.PaymentTerm + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.PersonCorpCode) != HelperClass.GetNUllorEmptystring(vend.PersonCorpCode))
                        {
                            vendorChanged = true;
                            statuschange.Append("PersonCorpCode :" + vprofVM.PersonCorpCode + " Changed to " + vend.PersonCorpCode + ". " + System.Environment.NewLine);
                        }
                        if (HelperClass.GetNUllorEmptystring(vprofVM.PersonSubTitle) != HelperClass.GetNUllorEmptystring(vend.PersonSubTitle))
                        {
                            vendorChanged = true;
                            statuschange.Append("PersonSubTitle :" + vprofVM.PersonSubTitle + " Changed to " + vend.PersonSubTitle + ". " + System.Environment.NewLine);
                        }
                        
                    StatusHistory sh = new StatusHistory(); 
                    VendorProfile vprofile = new VendorProfile();

                     v.Source = "Application";
                    // get it from session
                    vprofile.ID = vprof.ID;
                    vprofile.JDEVendorID = vprof.JDEVendorID;
                    vprofile.VRVENDORNUMBER = vprof.VRVENDORNUMBER;
                    vprofile.VendorGuid = vprof.VendorGuid;
                    vprofile.IsEinVerified = vprof.IsEinVerified ;
                    vprofile.RequestedUserEmail = vprof.RequestedUserEmail;
                    vprofile.SubmittedTime = vprof.SubmittedTime;
                    // get from VM
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorLegalName = string.IsNullOrEmpty(vend.VendorLegalName) ? vend.VendorDBAName : vend.VendorLegalName;
                    vprofile.VendorStatus = vend.VendorStatus;

                    vprofile.VendorDBAName = vend.VendorDBAName;
                    if(vprof.IsEinVerified == "Y")
                    {
                        vprofile.VendorEIN = vprof.VendorEIN;
                    }
                    else
                    {
                        vprofile.VendorEIN = vend.VendorEIN;
                    }
                     
                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorContactFN = vend.VContactFirstName;
                    // checking value
                    v.WriteEntry("*** " + vprofile.VendorEIN, EventLogEntryType.Information, 101, 1);

                    vprofile.VPhone = vend.VPhone;
                    vprofile.VFax = vend.VFax;
                    vprofile.VendorCity = vend.VendorCity;
                    vprofile.VendorState = vend.VendorState ?? "";
                    vprofile.VendorAddress1 = vend.VendorAddress1;
                    vprofile.VendorAddress2 = vend.VendorAddress2;
                    vprofile.VendorZipCode = vend.VendorZipCode;
                    vprofile.PersonCorpCode = vend.PersonCorpCode;
                    vprofile.PersonSubForm = vend.PersonSubForm;
                    vprofile.PersonSubTitle = vend.PersonSubTitle;
                    vprofile.UpdatedBy = SecurityUtility.GetSimpleUserName(User);
                    vprofile.LastUpdatedTime = DateTime.Now;
                    vprofile.AIName = vend.AIName;
                    vprofile.AIEmail = vend.AIEmail;
                    vprofile.AIPhone = vend.AIPhone;
                    vprofile.AIState = vend.AIState??"";
                    vprofile.AICity = vend.AICity;
                    vprofile.AIAddress = vend.AIAddress;
                    vprofile.AIAddress2 = vend.AIAddress2;
                    vprofile.AIZip = vend.AIZip;
                    vprofile.PaymentTerm = vend.PaymentTerm ?? "";
                    vprofile.StatusChangeReason = vend.StatusChangeReason;
                    vprofile.STATUSINJDE = vend.JDEStatus?? "";
                    vprofile.VendorType = vend.VendorType;
                    
                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";
                    vprofile.JDE = vend.JDE ? "Y" : "N";
                    vprofile.PROCORE = vend.PROCORE ? "Y" : "N";

                    vprofile.MRI1Code = vend.MRI1Code;
                    vprofile.MRI2Code = vend.MRI2Code;
                    vprofile.MRI12Code = vend.MRI12Code;
                    vprofile.YARDI5Code = vend.YARDI5Code;
                    vprofile.YARDI7Code = vend.YARDI7Code;
                    vprofile.YARDI8Code = vend.YARDI8Code;
                    vprofile.YARDI11Code = vend.YARDI11Code;
                    vprofile.YARDI14CODE = vend.YARDI14Code;

                    vprofile.CardContactName = vend.CardContactName;
                    vprofile.CardContactPhone = vend.CardContactPhone;
                    vprofile.CardEmailAddress = vend.CardContactEmail;


                    // save 
                    if (button.ToString().Trim() == "Save")
                    {
                        string filepath1 = ConfigurationManager.AppSettings["filepath"].ToString();
                        string filepath = filepath1 + vprof.ID.ToString() + "//";

                      
                        if (upload != null)
                        {
                            if (!Directory.Exists(filepath))
                            {
                                Directory.CreateDirectory(filepath);
                            }

                            // v.WriteEntry("Saving to " + path + vprof.ID.ToString(), EventLogEntryType.Information, 101, 1);
                            foreach (var file in upload)
                            {
                                string sv = filepath1 + vprof.ID.ToString() + "\\" + System.IO.Path.GetFileName(file.FileName);
                                file.SaveAs(sv);
                            }
                        }


                        IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);
                        TempData["AlertMessage"] = "Vendor update successful.";
                        v.WriteEntry("*** succesfully save", EventLogEntryType.Information, 101, 1);
                    }

                    if (vendorChanged)
                    {
                        if(vprofile.VendorType == "N")
                        {
                            vprofile.ProcoreUpdated = "N";
                        }

                        sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
                        sh.UpdatedTime = DateTime.Now;
                        sh.VStatus = vend.VendorStatus;
                        sh.Description = statuschange.ToString();
                        sh.VID = vprof.ID;
                        // 
                        EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);
                    }

                    if (button == "ExportJDE")
                    {

                        List<string> emails = new List<string>();

                        if(vend.VendorStatus == "InAccounting")
                        {
                            if (!string.IsNullOrEmpty(vprof.RequestedUserEmail))
                            {
                                emails.Add(vprof.RequestedUserEmail);
                            }
                        }
                        
                      
                        

                        vprofile.JDEVendorID = await ExportToJDE(vprofile);
                        TempData["AlertMessage"] = "Vendor Data has been moved to JDE. Please give one day to transfer.";

                        foreach (string s in Aemails)
                        {
                            emails.Add(s);
                        }
                        
                        string Subject = vend.VendorDBAName + " is exported to JDE. JDE Address Number : " + vprofile.JDEVendorID;
                       
                        await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, Subject, Subject);
                    }
                }
                catch(Exception e)
                {
                    TempData["AlertMessage"] = "There was an error saving the vendor profile, please try again.";

                    v.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
                    return RedirectToAction("VErrorPage");
                }
                // send email if send email has been pressed
                return RedirectToAction("VendorStatus","Admin");
            }
            else
            {
                
                TempData["AlertMessage"] = "Please fill all the details and please try again.";
                vend.Statuses = HelperClass.LoadStatuses();
                vend.States = HelperClass.LoadStates();
                vend.PaymentTerms = HelperClass.GetPayementTerms();
                vend.JDEStatuses = HelperClass.GetJDEVendorStatuses();
                IEnumerable<StatusHistory> statusHistList = Session["statushistlist"] as IEnumerable<StatusHistory>;
                vend.StatusHistoryList = statusHistList;
                vend.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
                vend.UploadfilesList = GetFilesFromFolder(vprof.ID.ToString());
                return View(vend);
            }

        }


        public async Task<ActionResult> ResendToCustomer(VendorProfileViewModel vend)
        {
            VendorProfile vp = Session["editvend"] as VendorProfile;
           // VendorProfileViewModel vprofVM = Session["VprofVM"] as VendorProfileViewModel;  

            StringBuilder statuschange = new StringBuilder();

            string emailuser = SecurityUtility.GetUserEmail(User);

            try
                {
                        string email2 = SecurityUtility.GetUserEmail(User);
                       
                        vp.RequestedUserEmail = email2;
                        vp.VendorEmail = vend.VendorEmail;
                        IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vp);

                        StringBuilder mailsub = new StringBuilder();

                        string body = string.Empty;
                        //using streamreader for reading my htmltemplate   
                        string loc = Server.MapPath("~/Views/EmailTemplates/InviteVendorTemplate.html");
                        using (StreamReader reader = new StreamReader(loc))
                        {
                            body = reader.ReadToEnd();
                        }

                        body = body.Replace("{SpecialInstructions}", "");
                        body = body.Replace("{SendEmailUrl}", SendEmailUrl); //replacing the required things  
                        body = body.Replace("{email}", vp.VendorGuid); //replacing the required things  



                        // Email to Client
                        //  string body = string.Format(mailsub.ToString(), vguid.ToString());
                        string Subject = "Ryan Companies - Vendor Setup Request-Reponse Needed";
                        List<string> emails = new List<string>();
                        emails.Add(vp.VendorEmail);


                        // add it to status history table
                        StatusHistory sh = new StatusHistory();
                        sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
                        sh.UpdatedTime = DateTime.Now;
                        sh.VStatus = vp.VendorStatus;
                        sh.Description = "Invite Resent to vendor on " + sh.UpdatedTime.ToString();
                        sh.VID = vp.ID;
                        EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);

                        if (vp.VendorStatus == HelperClass.InviteSent || vp.VendorStatus == HelperClass.Cancelled)
                        {
                            
                            await EmailNotificationUtility.SendEmailToClient(emails, vp.VendorGuid, vp.VendorDBAName, body, Subject);
                            TempData["AlertMessage"] = "Vendor Invite request successful.";
                            return RedirectToAction("AddVendor");
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Vendor is in Accounting process. Invite request cannot be sent.";
                            return View();

                        }

               


            }
                catch (Exception e)
                {
                    TempData["AlertMessage"] = "There was an error sending email to customer. Please try again after sometime.";
                return RedirectToAction("VErrorPage");
            }
            // send email if send email has been pressed
           // TempData["AlertMessage"] = "Email sent to Vendor.";
            //return RedirectToAction("AddVendor");

            }

        public async Task<ActionResult> ResendToAccounting(VendorProfileViewModel vend, IEnumerable<HttpPostedFileBase> upload)
        {
            VendorProfile vprof = Session["editvend"] as VendorProfile;
           // VendorProfileViewModel vprofVM = Session["VprofVM"] as VendorProfileViewModel;

            StringBuilder statuschange = new StringBuilder();

            string UpdateEmailUrl = ConfigurationManager.AppSettings["UpdateEmailUrl"].ToString();
            try
            {
                string email2 = SecurityUtility.GetUserEmail(User);
                vprof.StatusChangeReason = vend.StatusChangeReason;
                vprof.VendorStatus = HelperClass.InAccouting;
                vprof.RequestedUserEmail = email2;
                vprof.VendorEmail = vend.VendorEmail;
                IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprof);

                string body = string.Empty;
                //using streamreader for reading my htmltemplate   
                string loc = Server.MapPath("~/Views/EmailTemplates/EmailToAccouting.html");
                using (StreamReader reader = new StreamReader(loc))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{vendorname}", vprof.VendorDBAName);
                body = body.Replace("{UpdateEmailUrl}", UpdateEmailUrl); //replacing the required things  
                body = body.Replace("{vendorid}", vprof.ID.ToString()); //replacing the required things  
                body = body.Replace("{statuschangereason}", vend.StatusChangeReason);  //

              


                
                string Subject = "Ryan Companies - Vendor Update Request – Response Needed";
                List<string> emails = new List<string>();

                string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                string[] Aemails = AdminEmails.Split(',');
                foreach(string s in Aemails)
                {
                    emails.Add(s);
                }



                //if (!string.IsNullOrEmpty(vprof.RequestedUserEmail))
                //{
                //    emails.Add(vprof.RequestedUserEmail);
                //}
                

                //if (!string.IsNullOrEmpty(email2))
                //{
                //    emails.Add(email2);
                //}
                string filepath1 = ConfigurationManager.AppSettings["filepath"].ToString();
                string filepath = filepath1 + vprof.ID.ToString() + "//";

               

                if (upload != null)
                {
                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }
                    // v.WriteEntry("Saving to " + path + vprof.ID.ToString(), EventLogEntryType.Information, 101, 1);
                    foreach (var file in upload)
                    {
                        string sv = filepath1 + vprof.ID.ToString() + "\\" + System.IO.Path.GetFileName(file.FileName);
                        file.SaveAs(sv);
                    }
                }

                // add it to status history table
                StatusHistory sh = new StatusHistory();
                sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
                sh.UpdatedTime = DateTime.Now;
                sh.VStatus = vprof.VendorStatus;
                sh.Description = "Resend to Accounting on " + sh.UpdatedTime.ToString();
                sh.VID = vprof.ID;
                EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);

                await EmailNotificationUtility.SendEmailToClient(emails, vprof.VendorGuid.ToString(), vend.VendorDBAName, body, Subject);

               

                // add to status history
            }
            catch (Exception e)
            {
                TempData["AlertMessage"] = "There was an error sending email to customer. Please try again after sometime.";
                v.WriteEntry(e.Message, EventLogEntryType.Error, 101, 1);
                return RedirectToAction("VErrorPage");
            }
            // send email if send email has been pressed
            TempData["AlertMessage"] = "Email sent to Admin.";
            return RedirectToAction("AddVendor");
          

        }



        private async Task<string> ExportToJDE(VendorProfile vprofile)
        {
            bool actionType = false;
            
            VendorProfile vprofOLD = Session["editvend"] as VendorProfile;

            if (!System.IO.Directory.Exists(jdeFilePath))
            {
                System.IO.Directory.CreateDirectory(jdeFilePath);
            }

            if (string.IsNullOrEmpty(vprofile.JDEVendorID))
            {
                actionType = true;
                vprofile.JDEVendorID = GetNewEmployeeNumber();

            }

            if (string.IsNullOrEmpty(vprofile.VRVENDORNUMBER) && vprofile.VendorType =="V"  &&(! string.IsNullOrEmpty(vprofile.AIAddress) )&& (!string.IsNullOrEmpty(vprofile.AIState)))
            {
                vprofile.VRVENDORNUMBER = GetNewEmployeeNumber();
            }

            string filepa = string.Empty;
            if (vprofile.VendorType == "V")
            {
                filepa =  jdeFilePath + "\\" + "Vend_" + vprofile.JDEVendorID + ".txt";
            }
            else
            {
                filepa = jdeFilePath + "\\" + "Cust_" + vprofile.JDEVendorID + ".txt";
            }
            

            if (System.IO.File.Exists(filepa))
            {
                System.IO.File.Delete(filepa);
            }

            JdeJulianDateTool jd = new JdeJulianDateTool();
            jd.DateValue = DateTime.Now;

            StringBuilder headline= new StringBuilder();
            
            headline.Append("ACTIONTYPE;");   // EDBT alpha

            headline.Append("VENDORNUMBER;");   // NEXTNUMBER alpha
            headline.Append("VENDORNAME;");   // EDLN  number
            headline.Append("ADDR1;");   // EDSP alpha. Leave this field blank
            headline.Append("ADDR2;");   // TNAC alpha
            headline.Append("CITY;");   // MCU alpha
            headline.Append("STATE;");   // 
            headline.Append("ZIP;");    // zip


            headline.Append("PHONEPREFIX1OLD;");   // First 3 char
            headline.Append("PHONENUMBER1OLD;");   // Next 6 characters
            headline.Append("PHONETYPE1OLD;");   // TAX alpha
            headline.Append("PHONEPREFIX1NEW;");   // First 3 char
            headline.Append("PHONENUMBER1NEW;");   // Next 6 characters
            headline.Append("PHONETYPE1NEW;");   // TAX alpha

            headline.Append("PHONEPREFIX2OLD;");   // second  phone is FAX
            headline.Append("PHONENUMBER2OLD;");   // second  phone is FAX
            headline.Append("PHONETYPE2OLD;");   // second  phone is FAX
            headline.Append("PHONEPREFIX2NEW;");   // second  phone is FAX
            headline.Append("PHONENUMBER2NEW;");   // second  phone is FAX
            headline.Append("PHONETYPE2NEW;");   // second  phone is FAX


            headline.Append("EMAILOLD;");  // Email
            headline.Append("EMAILNEW;");  // Email

            headline.Append("TAXID;");  // TAXID
            headline.Append("PAYMENTTERMS;");

            headline.Append("FIRSTNAMEOLD;");
            headline.Append("MIDDLEINITIALOLD;");
            headline.Append("LASTAMEOLD;");
            headline.Append("TITLEOLD;");
            headline.Append("FIRSTNAMENEW;");
            headline.Append("MIDDLEINITIALNEW;");
            headline.Append("LASTAMENEW;");
            headline.Append("TITLENEW;");
            //
            headline.Append("VENDORDBANAME;");
            //
            headline.Append("AI_NAMEOLD;");
            headline.Append("AI_NAMENEW;");

            headline.Append("AI_PHONEPREFIXOLD;");
            headline.Append("AI_PHONENUMBEROLD;");
            headline.Append("AI_PHONETYPEOLD;");
            headline.Append("AI_PHONEPREFIXNEW;");
            headline.Append("AI_PHONENUMBERNEW;");
            headline.Append("AI_PHONETYPENEW;");

            headline.Append("AI_EMAILOLD;");
            headline.Append("AI_EMAILNEW;");

            headline.Append("VRVENDORNUMBEROLD;");
            headline.Append("PAY_ADDRESSOLD;");
            headline.Append("PAY_Address2OLD;");
            headline.Append("PAY_CityOLD;");
            headline.Append("PAY_StateOLD;");
            headline.Append("PAY_ZipCodeOLD;");

            headline.Append("VRVENDORNUMBERNEW;");
            headline.Append("PAY_ADDRESSNEW;");
            headline.Append("PAY_Address2NEW;");
            headline.Append("PAY_CityNEW;");
            headline.Append("PAY_StateNEW;");
            headline.Append("PAY_ZipCodeNEW;");
            headline.Append("VENDORSTATUS;");
            headline.Append("PERSONCORPCODE;");

            // AR name email phone
            headline.Append("AR_NAMEOLD;");
            headline.Append("AR_NAMENEW;");

            headline.Append("AR_PHONEPREFIXOLD;");
            headline.Append("AR_PHONENUMBEROLD;");
            headline.Append("AR_PHONETYPEOLD;");
            headline.Append("AR_PHONEPREFIXNEW;");
            headline.Append("AR_PHONENUMBERNEW;");
            headline.Append("AR_PHONETYPENEW;");

            headline.Append("AR_EMAILOLD;");
            headline.Append("AR_EMAILNEW;");



            headline.Append("UserID;");   // user alpha
            headline.Append("CURRENT DATE");
            


            using (var sw = new StreamWriter(filepa, true))
            {
                sw.WriteLine(headline.ToString());
                StringBuilder sb = new StringBuilder();

                // mandatory fields 
                if (actionType)
                {
                    sb.Append("A" + ";"); // number doesnot exist. so its add.
                }
                else
                {
                    sb.Append("C" + ";");  // number exists and so it is update
                }
                
                sb.Append(string.IsNullOrEmpty(vprofile.JDEVendorID) ? " ;": vprofile.JDEVendorID + ";");
                sb.Append( string.IsNullOrEmpty( vprofile.VendorLegalName) ? " ;": vprofile.VendorLegalName + ";");
                sb.Append( string.IsNullOrEmpty(vprofile.VendorAddress1)? " ;": vprofile.VendorAddress1 + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.VendorAddress2) ? " ;": vprofile.VendorAddress2+ ";");
                sb.Append(string.IsNullOrEmpty(vprofile.VendorCity) ? " ;" : vprofile.VendorCity + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.VendorState) ? " ;": vprofile.VendorState + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.VendorZipCode) ? " ;": vprofile.VendorZipCode + ";");

                //old phone number
                if (string.IsNullOrEmpty(vprofOLD.VPhone) || actionType )
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                   
                        sb.Append(vprofOLD.VPhone.Substring(0, 3) + ";");   // EDBT alpha
                        sb.Append(vprofOLD.VPhone.Substring(vprofOLD.VPhone.Length - 8) + ";");     // EDTN alpha
                        sb.Append(" ;");   // Phonetype blank. its business
                    
                    
                }

                //New number
                if (string.IsNullOrEmpty(vprofile.VPhone))
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofile.VPhone.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofile.VPhone.Substring(vprofile.VPhone.Length - 8) + ";");     // EDTN alpha
                    sb.Append(" ;");   // Phonetype blank. its business
                }


                // second OLD phone is FAX
                if (string.IsNullOrEmpty(vprofOLD.VFax) || actionType)
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofOLD.VFax.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofOLD.VFax.Substring(vprofOLD.VFax.Length - 8) + ";");     // EDTN alpha
                    sb.Append("FAX ;");   // Phonetype blank. its business
                }

                // second NEW phone is FAX
                if (string.IsNullOrEmpty(vprofile.VFax))
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofile.VFax.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofile.VFax.Substring(vprofile.VFax.Length - 8) + ";");     // EDTN alpha
                    sb.Append("FAX ;");   // Phonetype blank. its business
                }

                // OLD and New
                if(actionType)
                {
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.VendorEmail) ? " ;" : vprofOLD.VendorEmail + ";");
                }
                
                sb.Append(string.IsNullOrEmpty(vprofile.VendorEmail) ? " ;" : vprofile.VendorEmail + ";");


                sb.Append(string.IsNullOrEmpty(vprofile.VendorEIN)  ? " ;": vprofile.VendorEIN + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.PaymentTerm) ? " ;": "0"+ vprofile.PaymentTerm + ";");

                // old contact person
                if (actionType)
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.VendorContactFN) ? " ;" : vprofOLD.VendorContactFN + ";");
                    sb.Append(" ;");   // middle intial
                    sb.Append(string.IsNullOrEmpty(vprofOLD.VendorContactLN) ? " ;" : vprofOLD.VendorContactLN + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.PersonSubTitle) ? " ;" : vprofOLD.PersonSubTitle + ";");
                }
                    

                // New contact person
                sb.Append(string.IsNullOrEmpty(vprofile.VendorContactFN) ? " ;" : vprofile.VendorContactFN + ";");
                sb.Append(" ;");   // middle intial
                sb.Append(string.IsNullOrEmpty(vprofile.VendorContactLN) ? " ;" : vprofile.VendorContactLN + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.PersonSubTitle) ? " ;" : vprofile.PersonSubTitle + ";");


                sb.Append(string.IsNullOrEmpty(vprofile.VendorDBAName) ? " ;" : vprofile.VendorDBAName + ";");

                // AI old and new name
                if (actionType)
                {
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIName) ? " ;" : vprofOLD.AIName + ";");
                }
                    
                sb.Append(string.IsNullOrEmpty(vprofile.AIName) ? " ;" : vprofile.AIName + ";");

                // AI old phone
                if (string.IsNullOrEmpty(vprofOLD.AIPhone) || actionType)
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofOLD.AIPhone.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofOLD.AIPhone.Substring(vprofOLD.AIPhone.Length - 8) + ";");     // EDTN alpha
                    sb.Append(" ;");   // Phonetype blank. its business
                }
                // AI NEW phone
                if (string.IsNullOrEmpty(vprofile.AIPhone))
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofile.AIPhone.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofile.AIPhone.Substring(vprofile.AIPhone.Length - 8) + ";");     // EDTN alpha
                    sb.Append(" ;");   // Phonetype blank. its business
                }

                // old and new AI email
                if (actionType)
                {
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIEmail) ? " ;" : vprofOLD.AIEmail + ";");
                }
                sb.Append(string.IsNullOrEmpty(vprofile.AIEmail) ? " ;" : vprofile.AIEmail + ";");

                //  old info
                if (actionType)
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");


                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.VRVENDORNUMBER) ? " ;" : vprofOLD.VRVENDORNUMBER + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIAddress) ? " ;" : vprofOLD.AIAddress + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIAddress2) ? " ;" : vprofOLD.AIAddress2 + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AICity) ? " ;" : vprofOLD.AICity + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIState) ? " ;" : vprofOLD.AIState + ";");
                    sb.Append(string.IsNullOrEmpty(vprofOLD.AIZip) ? " ;" : vprofOLD.AIZip + ";");
                }
                

                // new info
                //if((vprofOLD.AIAddress != vprofile.AIAddress) || (vprofOLD.AIAddress2 != vprofile.AIAddress2) || (vprofOLD.AICity != vprofile.AICity) || (vprofOLD.AIState != vprofile.AIState) || (vprofOLD.AIZip != vprofile.AIZip))
                //{
                //    vprofile.VRVENDORNUMBER = GetNewEmployeeNumber();
                //}
               
                sb.Append(string.IsNullOrEmpty(vprofile.VRVENDORNUMBER) ? " ;" : vprofile.VRVENDORNUMBER + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.AIAddress) ? " ;" : vprofile.AIAddress + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.AIAddress2) ? " ;" : vprofile.AIAddress2 + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.AICity) ? " ;" : vprofile.AICity + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.AIState) ? " ;" : vprofile.AIState + ";");
                sb.Append(string.IsNullOrEmpty(vprofile.AIZip) ? " ;" : vprofile.AIZip + ";");

                sb.Append(vprofile.STATUSINJDE == "InActive" ? "I;" : "A;" );
                sb.Append(string.IsNullOrEmpty(vprofile.PersonCorpCode) ? " ;" : vprofile.PersonCorpCode + ";");

                // AR old and new name
                if (actionType)
                {
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.CardContactName) ? " ;" : vprofOLD.CardContactName + ";");
                }

                sb.Append(string.IsNullOrEmpty(vprofile.CardContactName) ? " ;" : vprofile.CardContactName + ";");

                // AI old phone
                if (string.IsNullOrEmpty(vprofOLD.CardContactPhone) || actionType)
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(vprofOLD.CardContactPhone.Substring(0, 3) + ";");   // EDBT alpha
                    sb.Append(vprofOLD.CardContactPhone.Substring(vprofOLD.CardContactPhone.Length - 8) + ";");     // EDTN alpha
                    sb.Append(" ;");   // Phonetype blank. its business
                }
                // AI NEW phone
                if (string.IsNullOrEmpty(vprofile.CardContactPhone))
                {
                    sb.Append(" ;");
                    sb.Append(" ;");
                    sb.Append(" ;");
                }
                else
                {
                    if(vprofile.CardContactPhone.Length >10)
                    {
                        sb.Append(vprofile.CardContactPhone.Substring(0, 3) + ";");   // EDBT alpha
                        sb.Append(vprofile.CardContactPhone.Substring(vprofile.CardContactPhone.Length - 8) + ";");     // EDTN alpha
                        sb.Append(" ;");   // Phonetype blank. its business
                    } else
                    {
                        sb.Append(" ;");
                    }
                  
                }

                // old and new AI email
                if (actionType)
                {
                    sb.Append(" ;");
                }
                else
                {
                    sb.Append(string.IsNullOrEmpty(vprofOLD.CardEmailAddress) ? " ;" : vprofOLD.CardEmailAddress + ";");
                }
                sb.Append(string.IsNullOrEmpty(vprofile.CardEmailAddress) ? " ;" : vprofile.CardEmailAddress + ";");





                sb.Append(vprofile.ID.ToString() + ";");   // user alpha
                sb.Append(jd.DateValue.ToString());
              

                // writeline
                sw.WriteLine(sb.ToString());

            }

            // save 
            if(vprofile.VendorStatus == HelperClass.InviteSent || vprofile.VendorStatus == HelperClass.InAccouting)
            {
                vprofile.VendorStatus = "Approved";
                
            }

            if(vprofile.STATUSINJDE == "InActive")
            {
                vprofile.STATUSINJDE = "InActive";
                vprofile.VendorStatus = "Cancelled";
            }
            else
            {
                vprofile.STATUSINJDE = "Active";
            }


            if(vprofile.PROCORE == "Y")
            {
                vprofile.ProcoreUpdated = "N";
            }
            

            
            IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);

            // add it to status history table
            StatusHistory sh = new StatusHistory();
            sh.UpdatedBy = SecurityUtility.GetSimpleUserName(User) ?? "Admin";
            sh.UpdatedTime = DateTime.Now;
            sh.VStatus = vprofile.VendorStatus;
            sh.Description = "Exported to JDE on " + sh.UpdatedTime.ToString();
            sh.VID = vprofile.ID;
            EFDbActionResult saveStatusHist = await statusHistoryRepo.SaveStatusHistoryByID(sh);


            return vprofile.JDEVendorID;

        }

       


        public ActionResult VErrorPage()
        {
            return View();
        }

        /// <summary>
        /// Get Files from folder.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<FileDetails> GetFilesFromFolder(string id)
        {
            List<string> files = new List<string>();
            List<FileDetails> uFiles = new List<FileDetails>();
            //string webRootPath = Server.MapPath("~");
            //string filepath1 = "/VendorDocuments/" + id + "/";
            //string docPath = Path.GetFullPath(Path.Combine(webRootPath, filepath1));



             string filepath = folderfilepath + "//" + id + "//";
            // string filepath = folderfilepath + id + "//";
           // string s2 =  HostingEnvironment.MapPath(filepath);
           // string s1 =  HttpContext.Request.Url.ToString() + "//docs//" + id + "//"; 
            

            if (Directory.Exists(filepath))
            {
                foreach( string s in Directory.GetFiles(filepath))
                {
                    FileDetails ff = new FileDetails();
                    ff.UploadFileName = Path.GetFileName(s);
                    ff.UploadFilePath = documentUrl + id + "//" + Path.GetFileName(s);
                    ff.UploadFileDate = System.IO.File.GetCreationTime(s).ToShortDateString();
                    uFiles.Add(ff);
                    //files.Add(filepath + Path.GetFileName(s));
                    files.Add(documentUrl + id + "//" + Path.GetFileName(s));
                    
                }
              //  files = System.IO.Directory.GetFiles(filepath).ToList();
            }
            //else
            //{
            //   Directory.CreateDirectory(filepath);
            //}
           

            return uFiles;
           
        }

        private string GetOraDB()
        {
            StringBuilder sbOraDb = new StringBuilder();

            //sbOraDb.Append("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=rydrac.ryan.local)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=rydrac)));");
            //sbOraDb.Append("User Id=onboardapp;Password=OnBoard;");

            sbOraDb.Append("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=");
            sbOraDb.Append(OracleHostName);
            sbOraDb.Append(")(PORT=");
            sbOraDb.Append(OraclePort);
            sbOraDb.Append("))(CONNECT_DATA= (SERVER = DEDICATED) (SERVICE_NAME=");
            sbOraDb.Append(OracleDbName);
            sbOraDb.Append(")));");
            sbOraDb.Append("User Id=");
            sbOraDb.Append(OracleUserName);
            sbOraDb.Append(";Password=");
            sbOraDb.Append(OraclePwd);
            sbOraDb.Append(";");


            return sbOraDb.ToString();
        }

        private string GetNewEmployeeNumber()
        {
            Decimal empNumber = 0;

            string oradb = GetOraDB();

            using (OracleConnection conn = new OracleConnection(oradb))
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand())
                {
                    cmd.Connection = conn;
                    // start transaction
                    OracleTransaction transaction = conn.BeginTransaction(IsolationLevel.Serializable);
                    cmd.Transaction = transaction;

                    try
                    {
                        // First step get next number
                        cmd.CommandText = BuildSelectEmployeeNumberSql(environment);
                        cmd.CommandType = CommandType.Text;

                        OracleDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            empNumber = dr.GetDecimal(2);
                        }

                        // Update the NextNumber table
                        Decimal newNextNumber = empNumber + 1;
                        cmd.CommandText = BuildUpdateEmployeeNumberSql(newNextNumber, environment);
                        cmd.ExecuteNonQuery(); 
                        // Commit
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return Convert.ToString(empNumber);
        }

        


        public static string BuildSelectEmployeeNumberSql(string environment)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append("SELECT nnsy, nnud01, nnn001 ");
            sbSql.Append("FROM ");
            //sbSql.Append(GetSchemaNameByEnvironment(environment, true));
            sbSql.Append(environment + ".f0002 ");
            sbSql.Append("WHERE nnsy = '01'");

            return sbSql.ToString();
        }

        private static string BuildUpdateEmployeeNumberSql(Decimal nextNumber, string environment)
        {
            StringBuilder sbSql = new StringBuilder();

            sbSql.Append("UPDATE ");
           // sbSql.Append(GetSchemaNameByEnvironment(environment, true));
            sbSql.Append(environment + ".f0002 ");
            sbSql.Append("SET NNN001 = ");
            sbSql.Append(nextNumber);
            sbSql.Append(" WHERE NNSY = '01'");

            return sbSql.ToString();
        }


        private IEnumerable<string> ChangedFields<T>(T first, T second)
        {
            if (first.GetType() != second.GetType())
                throw new ArgumentOutOfRangeException("Objects should be of the same type");

            var properties = first
                .GetType()
                .GetProperties();

            foreach (var property in properties)
            {
                if (!object.Equals(property.GetValue(first), property.GetValue(second)))
                {
                    yield return property.Name;
                }
            }
        }


    }
}