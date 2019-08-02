using Ryan.VendorManagement.WebUI.Models.vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using sw=System.Web;
using System.Web.Mvc;
using Ryan.VendorManagement.WebUI.Infrastructure;
using Ryan.VendorOnboarding.Domain.Entities;
using Ryan.VendorManagement.WebUI.Infrastructure.Security;
using Ryan.VendorOnboarding.Domain;
using Ryan.VendorOnboarding.Domain.Concrete;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Web.Hosting;
using Ryan.VendorManagement.WebUI.Infrastructure.Utilities;
using System.Diagnostics;

namespace Ryan.VendorManagement.WebUI.Controllers
{
    public class CreateController : Controller
    {
        EFVendorProfileRepositary vendorProfileRepository ;
        string cdfcfilepath = ConfigurationManager.AppSettings["cdfcfilepath"].ToString();
        SecurityCheckResult securityCheckResult = null;
        string SendEmailUrl = ConfigurationManager.AppSettings["SendEmailUrl"].ToString();
        private const int fileMegaBytes = 20 * 1024 * 1024;

        public CreateController()
        {
            vendorProfileRepository = new EFVendorProfileRepositary();
           
            
        }


        // GET: Create
        public async Task<ActionResult> Index()
        {


            securityCheckResult = await SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser");

            if (!securityCheckResult.Valid)
            {
                return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
            }

            return View();
            

        }


        // GET: Create/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Create/Create
        public ActionResult CreateVendor()
        {
            //VendorCreateViewModel v = new VendorCreateViewModel();

            //VendorProfileViewModel v1 = new VendorProfileViewModel();
            //v1.States = HelperClass.LoadStates();
            //v1.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            //v1.PaymentTerms = HelperClass.GetPayementTerms();

            //VendorProfileViewModel v2 = new VendorProfileViewModel();
            //v2.States = HelperClass.LoadStates();
            //v2.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            //v2.PaymentTerms = HelperClass.GetPayementTerms();

            //VendorProfileViewModel v3 = new VendorProfileViewModel();
            //v3.States = HelperClass.LoadStates();
            //v3.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            //v3.PaymentTerms = HelperClass.GetPayementTerms();

            //v.VendorProfileViewModel = v1;
            //v.CustomerProfileViewModel = v2;
            //v.NonPaidVendorProfileViewModel = v3;

            return RedirectToAction("CreateVendorRecord", "Create");
        }

        
        [HttpPost]
        public async Task<ActionResult> CreateVendorRecord(VendorProfileViewModel vendCreate, IEnumerable<sw.HttpPostedFileBase> upload , sw.HttpPostedFileBase upload1)
        {

            EventLog v = new EventLog("Application");
            try
            {
                if (ModelState.IsValid)
                {
                    v.Source = "Application";
                    v.WriteEntry("Started in vendor create - ", EventLogEntryType.Information, 101, 1);

                    bool upfile = false;

                    if(upload != null)
                    {
                        foreach(var fi in upload)
                        {
                            if(fi.ContentLength > fileMegaBytes)
                            {
                                upfile = true;
                            }
                        }
                    }

                    if(upload1 != null && upload1.ContentLength > fileMegaBytes)
                    {
                        upfile = true;
                    }

                    if (upfile)
                    {
                        vendCreate.States = HelperClass.LoadStates();
                        vendCreate.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
                        vendCreate.PaymentTerms = HelperClass.GetPayementTerms();

                        TempData["AlertMessage"] = "You cannot upload files more than 20 MB.";
                        return View(vendCreate);
                    }


                    VendorProfileViewModel vend = vendCreate;

                    // user details
                    string emailuser = SecurityUtility.GetUserEmail(User);

                    VendorProfile vprofile = new VendorProfile();
                    vprofile.VendorGuid = Guid.NewGuid().ToString();
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorLegalName = string.IsNullOrEmpty( vend.VendorLegalName) ? vend.VendorDBAName : vend.VendorLegalName;
                    vprofile.VendorStatus = HelperClass.InAccouting;
                    vprofile.VendorContactFN = vend.VContactFirstName;
                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorDBAName = vend.VendorDBAName;
                    vprofile.VendorEIN = vend.VendorEIN;
                    vprofile.VPhone = vend.VPhone;
                    vprofile.VFax = vend.VFax;
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorCity = vend.VendorCity;
                    vprofile.VendorState = string.IsNullOrEmpty(vend.VendorState) ? "" : vend.VendorState;
                    vprofile.VendorAddress1 = vend.VendorAddress1;
                    vprofile.VendorAddress2 = vend.VendorAddress2;
                    vprofile.VendorZipCode = vend.VendorZipCode;
                    vprofile.PersonCorpCode = string.IsNullOrEmpty(vend.PersonCorpCode) ? "" : vend.PersonCorpCode;
                    vprofile.PersonSubForm = vend.PersonSubForm;
                    vprofile.PersonSubTitle = vend.PersonSubTitle;
                    vprofile.UpdatedBy = SecurityUtility.GetSimpleUserName(User);

                    if(!string.IsNullOrEmpty(vend.PersonSubForm))
                    {
                        string[] tokens = vend.PersonSubForm.Split(new[] { ' ' }, 2);
                      
                        vprofile.VendorContactFN = tokens[0];
                        if (tokens.Length > 1)
                        {
                            vprofile.VendorContactLN = tokens[1];
                        }
                    }


                    v.WriteEntry("Started in vendor create - ", EventLogEntryType.Information, 101, 1);

                    vprofile.SourceType = "RyanInvite";
                    vprofile.STATUSINJDE = "NotInJDE";
                    vprofile.VendorStatus = HelperClass.InAccouting;
                    vprofile.VendorType = HelperClass.Vendor;
                    vprofile.IsEinVerified = vend.IsEINValid ? "Y" : "N";
                    vprofile.AIName = vend.AIName;
                    vprofile.AIEmail = vend.AIEmail;
                    vprofile.AIPhone = vend.AIPhone;
                    vprofile.AIState = string.IsNullOrEmpty(vend.AIState) ? "" : vend.AIState;
                    vprofile.AICity = vend.AICity;
                    vprofile.AIAddress = vend.AIAddress;
                    vprofile.AIAddress2 = vend.AIAddress2;
                    vprofile.AIZip = vend.AIZip;
                    vprofile.PaymentTerm = string.IsNullOrEmpty(vend.PaymentTerm) ? "" : vend.PaymentTerm;
                    vprofile.StatusChangeReason = vend.StatusChangeReason;

                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";
                    vprofile.RequestedUserEmail = emailuser;
                    vprofile.JDE = "Y";

                    if(SecurityUtility.CheckUserRole(User) == "ConstructionUser")
                    {
                        vprofile.PROCORE = "Y";
                    }
                    else
                    {
                        vprofile.PROCORE = vend.PROCORE ? "Y" : "N";
                    }
                    vprofile.SubmittedTime = DateTime.Now;
                    if (vprofile.PROCORE == "Y")
                    {
                        vprofile.ProcoreUpdated = "N";
                    }

                    v.WriteEntry("Saving in vendor create - ", EventLogEntryType.Information, 101, 1);
                    //  Save Vendor
                    EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);

                    if (saveResult.Errors.Count() > 0)
                    {
                        return RedirectToAction("VErrorPage", "VendorManagement");
                    }

                    vprofile.ID = saveResult.SavedID;
                    // string filepath = path + vprofile.ID + "//";
                    // string s2 = HostingEnvironment.MapPath(filepath);

                    string filepath1 = ConfigurationManager.AppSettings["filepath"].ToString();
                    string filepath = filepath1 + vprofile.ID.ToString() + "//";


                   // v.WriteEntry("checking folders in vendor create - ", EventLogEntryType.Information, 101, 1);

                   

                    if (upload != null)
                    {
                        if (!Directory.Exists(filepath))
                        {
                            Directory.CreateDirectory(filepath);
                        }

                        foreach (var file in upload)
                        {
                            string sv = filepath1 + vprofile.ID.ToString() + "\\" + System.IO.Path.GetFileName(file.FileName);
                            v.WriteEntry("File path in vendor create - " + sv, EventLogEntryType.Information, 101, 1);
                            file.SaveAs(sv);
                        }
                    }

                    string fileattach = "";

                   

                    if (upload1 != null)
                    {
                        if (!Directory.Exists(cdfcfilepath + vprofile.ID.ToString()))
                        {
                            Directory.CreateDirectory(cdfcfilepath + vprofile.ID.ToString());
                        }

                        string sv = cdfcfilepath + vprofile.ID.ToString() + "\\" + System.IO.Path.GetFileName(upload1.FileName);
                        upload1.SaveAs(sv);
                        fileattach = sv;
                    }


                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    string loc = Server.MapPath("~/Views/EmailTemplates/InviteVendorTemplate.html");
                    using (StreamReader reader = new StreamReader(loc))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{SendEmailUrl}", SendEmailUrl); //replacing the required things  
                    body = body.Replace("{email}", vprofile.VendorGuid); //replacing the required things  

                    List<string> emails = new List<string>();
                    string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                    string[] Aemails = AdminEmails.Split(',');
                    
                    foreach (string s in Aemails)
                    {
                        emails.Add(s);
                    }

                    v.WriteEntry("Sending email in vendor create", EventLogEntryType.Information, 101, 1);

                    string Subject = ConfigurationManager.AppSettings["emailsub2"].ToString();

                    Subject = Subject.Replace("{0}",vend.VendorDBAName);

                    string emailbody = "New Vendor : " + vend.VendorDBAName + " is added to VendorOnboarding System.";
                    if (!string.IsNullOrEmpty(fileattach))
                    {
                        emailbody = emailbody + " Vendor also submitted a Certified Diverse Firm Certificate for review.”";
                    }

                    await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, emailbody, Subject,fileattach);

                    TempData["AlertMessage"] = "Vendor is created. ";

                }
                else
                {

                    vendCreate.States = HelperClass.LoadStates();
                    vendCreate.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
                    vendCreate.PaymentTerms = HelperClass.GetPayementTerms();
                    return View(vendCreate);
                }

              

                return RedirectToAction("CreateVendorRecord", "Create");
            }
            catch(Exception ex)
            {
                v.WriteEntry(ex.InnerException.StackTrace, EventLogEntryType.Error, 101, 1);
                v.WriteEntry(ex.InnerException.Source, EventLogEntryType.Error, 101, 1);
                v.WriteEntry(ex.InnerException.Message, EventLogEntryType.Error, 101, 1);
                return RedirectToAction("VErrorPage", "VendorManagement");
            }
        }


        [HttpGet]
        public ActionResult CreateVendorRecord()
        {
            if (securityCheckResult is null)
            {
                securityCheckResult =  SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser").Result;

                if (!securityCheckResult.Valid)
                {
                    return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
                }
            }

            VendorProfileViewModel v1 = new VendorProfileViewModel();
            v1.States = HelperClass.LoadStates();
            v1.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            v1.PaymentTerms = HelperClass.GetPayementTerms();
            return View(v1);
        }

        [HttpGet]
        public  ActionResult CreateNonPaidVendor()
        {
            if (securityCheckResult is null)
            {
                securityCheckResult = SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser").Result;

                if (!securityCheckResult.Valid)
                {
                    return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
                }
            }


            VendorProfileViewModel v1 = new VendorProfileViewModel();
            v1.States = HelperClass.LoadStates();
            v1.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            v1.PaymentTerms = HelperClass.GetPayementTerms();
            return View(v1);
        }

        [HttpPost]
        public async Task<ActionResult> CreateNonPaidVendor(VendorProfileViewModel vendCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    VendorProfileViewModel vend = vendCreate;
                    // user details
                    string emailuser = SecurityUtility.GetUserEmail(User);


                    VendorProfile vprofile = new VendorProfile();

                    vprofile.VendorGuid = Guid.NewGuid().ToString();

                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorLegalName = string.IsNullOrEmpty(vend.VendorLegalName) ? vend.VendorDBAName : vend.VendorLegalName;

                    vprofile.VendorContactFN = vend.VContactFirstName;
                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorDBAName = vend.VendorDBAName;
                    vprofile.VendorEIN = vend.VendorEIN;
                    vprofile.VPhone = vend.VPhone;
                    vprofile.VFax = vend.VFax;
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorCity = vend.VendorCity;
                    vprofile.PersonCorpCode = string.Empty;
                    vprofile.VendorAddress1 = vend.VendorAddress1;
                    vprofile.VendorAddress2 = vend.VendorAddress2;
                    vprofile.VendorZipCode = vend.VendorZipCode;
                    vprofile.VendorState = string.IsNullOrEmpty(vend.VendorState) ? "" : vend.VendorState;
                    vprofile.PersonSubForm = vend.PersonSubForm;
                    vprofile.PersonSubTitle = vend.PersonSubTitle;
                    vprofile.UpdatedBy = SecurityUtility.GetSimpleUserName(User);
                    vprofile.LastUpdatedTime = DateTime.Now;
                    vprofile.ProcoreUpdated = "N";
                    if (!string.IsNullOrEmpty(vend.PersonSubForm))
                    {
                        string[] tokens = vend.PersonSubForm.Split(new[] { ' ' }, 2);

                        vprofile.VendorContactFN = tokens[0];
                        if (tokens.Length > 1)
                        {
                            vprofile.VendorContactLN = tokens[1];
                        }
                    }


                    vprofile.IsEinVerified ="Y" ;
                    vprofile.AIName = vend.AIName;
                    vprofile.AIEmail = vend.AIEmail;
                    vprofile.AIPhone = vend.AIPhone;
                    vprofile.AIState = string.IsNullOrEmpty(vend.AIState) ? "" : vend.AIState;
                    vprofile.AICity = vend.AICity;
                    vprofile.AIAddress = vend.AIAddress;
                    vprofile.AIAddress2 = vend.AIAddress2;
                    vprofile.AIZip = vend.AIZip;
                    vprofile.PaymentTerm = vend.PaymentTerm;
                    vprofile.StatusChangeReason = vend.StatusChangeReason;
                    vprofile.VendorType = HelperClass.NonPaid;
                    vprofile.RequestedUserEmail = emailuser;

                    vprofile.SourceType = "RyanInvite";
                    vprofile.STATUSINJDE = "NotInJDE";
                    vprofile.VendorStatus = HelperClass.Approved;

                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";
                   
                    vprofile.SubmittedTime = DateTime.Now;

                    vprofile.JDE = "N";
                    vprofile.PROCORE = "Y";

                    //  Save Vendor
                    EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);

                    vprofile.ID = saveResult.SavedID;

                   // string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                   // string loc = Server.MapPath("~/Views/EmailTemplates/InviteVendorTemplate.html");
                    //using (StreamReader reader = new StreamReader(loc))
                    //{
                    //    body = reader.ReadToEnd();
                    //}

                    //body = body.Replace("{SendEmailUrl}", SendEmailUrl); //replacing the required things  
                    //body = body.Replace("{email}", vprofile.VendorGuid); //replacing the required things  

                    //List<string> emails = new List<string>();
                    //string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                    //string[] Aemails = AdminEmails.Split(',');

                    //foreach (string s in Aemails)
                    //{
                    //    emails.Add(s);
                    //}
                    ////if (!string.IsNullOrEmpty(emailuser))
                    ////{
                    ////    emails.Add(emailuser);
                    ////}
                    //string Subject = "New Non-Paid Vendor : " + vend.VendorDBAName + " is added to VendorOnboarding System.";
                    //await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, Subject, Subject);

                    TempData["AlertMessage"] = "Non paid Vendor is created.";

                    return RedirectToAction("CreateNonPaidVendor", "Create");
                }
                catch
                {
                    TempData["AlertMessage"] = "There is error while creating Non paid Vendor. ";
                    return RedirectToAction("VErrorPage", "VendorManagement");
                }
            }
            else
            {
                vendCreate.States = HelperClass.LoadStates();

                return View(vendCreate);
            }


             
        }



        [HttpGet]
        public ActionResult CreateCustomerVendor()
        {
            if (securityCheckResult is null)
            {
                securityCheckResult = SecurityUtility.CheckUserRights(User, "GeneralUser,AdminUser").Result;

                if (!securityCheckResult.Valid)
                {
                    return RedirectToAction(securityCheckResult.ActionName, securityCheckResult.ControllerName);
                }
            }

            VendorProfileViewModel v1 = new VendorProfileViewModel();
            v1.States = HelperClass.LoadStates();
            v1.PersonCorpCodes = HelperClass.GetPersonCorpCodes();
            v1.PaymentTerms = HelperClass.GetPayementTerms();
            return View(v1);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomerVendor(VendorProfileViewModel vendCreate)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    VendorProfileViewModel vend = vendCreate;
                    // user details
                    string emailuser = SecurityUtility.GetUserEmail(User);


                    VendorProfile vprofile = new VendorProfile();

                    vprofile.VendorGuid = Guid.NewGuid().ToString();

                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorLegalName = string.IsNullOrEmpty(vend.VendorLegalName) ? vend.VendorDBAName : vend.VendorLegalName;

                    vprofile.VendorContactFN = vend.VContactFirstName;
                    vprofile.VendorContactLN = vend.VContactLastName;
                    vprofile.VendorDBAName = vend.VendorDBAName;
                    vprofile.VendorEIN = vend.VendorEIN;
                    vprofile.VPhone = vend.VPhone;
                    vprofile.VFax = vend.VFax;
                    vprofile.VendorEmail = vend.VendorEmail;
                    vprofile.VendorCity = vend.VendorCity;
                    vprofile.VendorState = string.IsNullOrEmpty(vend.VendorState) ? "" : vend.VendorState;
                    vprofile.VendorAddress1 = vend.VendorAddress1;
                    vprofile.VendorAddress2 = vend.VendorAddress2;
                    vprofile.VendorZipCode = vend.VendorZipCode;
                    vprofile.IsEinVerified = "Y";
                    vprofile.PersonSubForm = vend.PersonSubForm;
                    vprofile.PersonSubTitle = vend.PersonSubTitle;
                    vprofile.SubmittedTime = DateTime.Now;
                    vprofile.SourceType = "RyanInvite";
                    vprofile.STATUSINJDE = "NotInJDE";
                    vprofile.VendorStatus = HelperClass.InAccouting;
                    vprofile.AIState = string.IsNullOrEmpty(vend.AIState) ? "" : vend.AIState;

                    if (!string.IsNullOrEmpty(vend.PersonSubForm))
                    {
                        string[] tokens = vend.PersonSubForm.Split(new[] { ' ' }, 2);

                        vprofile.VendorContactFN = tokens[0];
                        if (tokens.Length > 1)
                        {
                            vprofile.VendorContactLN = tokens[1];
                        }
                    }



                    vprofile.UpdatedBy = SecurityUtility.GetSimpleUserName(User);
                    vprofile.LastUpdatedTime = DateTime.Now;
                    vprofile.VendorType = HelperClass.Customer;

                    vprofile.AIName = vend.AIName;
                    vprofile.AIEmail = vend.AIEmail;
                    vprofile.AIPhone = vend.AIPhone;
                    vprofile.AIState = vend.AIState;
                    vprofile.AICity = vend.AICity;
                    vprofile.AIAddress = vend.AIAddress;
                    vprofile.AIAddress2 = vend.AIAddress2;
                    vprofile.AIZip = vend.AIZip;
                    vprofile.PaymentTerm = string.IsNullOrEmpty(vend.PaymentTerm) ? "" : vend.PaymentTerm;
                    vprofile.StatusChangeReason = vend.StatusChangeReason;
                    vprofile.RequestedUserEmail = emailuser;
                    vprofile.MRI1 = vend.MRI1 ? "Y" : "N";
                    vprofile.MRI2 = vend.MRI2 ? "Y" : "N";
                    vprofile.MRI10 = vend.MRI10 ? "Y" : "N";
                    vprofile.MRI12 = vend.MRI12 ? "Y" : "N";
                    vprofile.YARDI5 = vend.YARDI5 ? "Y" : "N";
                    vprofile.YARDI14 = vend.YARDI14 ? "Y" : "N";
                    vprofile.YARDI7 = vend.YARDI7 ? "Y" : "N";
                    vprofile.YARDI8 = vend.YARDI8 ? "Y" : "N";
                    vprofile.YARDI11 = vend.YARDI11 ? "Y" : "N";

                    vprofile.JDE = "Y";
                    vprofile.PROCORE = "Y";

                    vprofile.ProcoreUpdated = "Y";


                    //  Save Vendor
                    EFDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);

                    vprofile.ID = saveResult.SavedID;

                    string body = string.Empty;
                    //using streamreader for reading my htmltemplate   
                    string loc = Server.MapPath("~/Views/EmailTemplates/InviteVendorTemplate.html");
                    using (StreamReader reader = new StreamReader(loc))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("{SendEmailUrl}", SendEmailUrl); //replacing the required things  
                    body = body.Replace("{email}", vprofile.VendorGuid); //replacing the required things  

                    List<string> emails = new List<string>();
                    string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                    string[] Aemails = AdminEmails.Split(',');

                    foreach (string s in Aemails)
                    {
                        emails.Add(s);
                    }
                    //if(!string.IsNullOrEmpty(emailuser))
                    //{
                    //    emails.Add(emailuser);
                    //}

                   // string Subject = "New customer : " + vend.VendorDBAName + " is added to VendorOnboarding System.";

                    string Subject = ConfigurationManager.AppSettings["emailsub3"].ToString();

                    Subject = Subject.Replace("{0}", vend.VendorDBAName);

                    await EmailNotificationUtility.SendEmailToClient(emails, "", vend.VendorDBAName, Subject, Subject);

                    TempData["AlertMessage"] = "Customer is created. ";

                    return RedirectToAction("CreateCustomerVendor", "Create");
                }
                catch
                {
                    TempData["AlertMessage"] = "There is error while creating customer. ";
                    return RedirectToAction("VErrorPage", "VendorManagement");
                }
            }
            else
            {
                vendCreate.States = HelperClass.LoadStates();
               
                return View(vendCreate);
            }

               
        }

        // GET: Create/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

      

    
    }
}
