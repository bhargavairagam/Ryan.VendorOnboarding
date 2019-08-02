using Newtonsoft.Json;
using Ryan.VendorOnboarding.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ryan.VendorOnboarding.WebUI.Infrastructure;
using System.Web.Hosting;
using Newtonsoft.Json.Linq;


namespace Ryan.VendorOnboarding.WebUI.Controllers
{
    public class HomeController : Controller
    {
       string urlpath = ConfigurationManager.AppSettings["webapiurl"].ToString();
        string path = ConfigurationManager.AppSettings["folderfilepath"].ToString();
        string filepath1 = ConfigurationManager.AppSettings["filepath"].ToString();

        // private  string path = @"C:\Users\biragam\Documents\VendorDocs\";
     //   string path = ConfigurationManager.AppSettings["filepath"].ToString();

        public async Task<ActionResult> Index(string id)
        {
            // LoadCountries();

            VendorViewModel vmodel = new VendorViewModel();
            vmodel.States = LoadStates();
            vmodel.PaymentTerms = GetPayementTerms();
            vmodel.PersonCorpCodes = GetPersonCorpCodes();
            TempData["Message"] = "";
            // for testing purpose
            // id = "e3f467b1-476a-4474-bdce-564f4c96a771";
            if (string.IsNullOrEmpty(id)){

                TempData["Message"] = "Please check proper URL or link in email from Ryan Companies US, INC.Thanks!!!.";
               return  RedirectToAction("ThankYou");

            }

            try
            {
                string getURL = urlpath + "Vendor/GetProfileByGuid?guid=" + id;

                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(getURL);
                
               // var response = await httpClient.GetAsync(devURL + id + "&status=EmailSent");
                //will throw an exception if not successful
                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

               
                // redirect to error message
                if ( String.IsNullOrWhiteSpace(content) || content == "null")
                {
                    
                    return RedirectToAction("VErrorPage");
                }


                VendorProfile prof = JsonConvert.DeserializeObject<VendorProfile>(content);
                Session["vp"] = prof;

                if(prof.VendorStatus.ToLower() == HelperClass.InviteSent.ToLower() || prof.VendorStatus.ToLower() == HelperClass.Resend.ToLower() )
                {
                    vmodel.VendorName = prof.VendorLegalName;
                    vmodel.VendorEmail = prof.VendorEmail;
                    vmodel.VendorDBAName = prof.VendorDBAName;
                    vmodel.VendorEIN = prof.VendorEIN;
                    vmodel.VendorEmail = prof.VendorEmail;
                    vmodel.VendorPhone = prof.VPhone;
                    vmodel.VendorAddress = prof.VendorAddress1;
                    vmodel.VendorAddress2 = prof.VendorAddress2;
                    vmodel.VendorCity = prof.VendorCity;
                    vmodel.VendorState = prof.VendorState;
                    vmodel.VendorZip = prof.VendorZipCode;
                    vmodel.VendorFax = prof.VFax;
                    vmodel.PersonCompletingForm = prof.VendorContactFN + " " + prof.VendorContactLN;
                    vmodel.PersonCompletingTitle = prof.PersonSubTitle;
                    vmodel.AIAddress = prof.AIAddress;
                    vmodel.AIAddress2 = prof.AIAddress2;
                    vmodel.AICity = prof.AICity;
                    vmodel.AIZip = prof.AIZip;
                    vmodel.AIState = prof.AIState;
                    vmodel.PersonCorpCode = prof.PersonCorpCode;
                    vmodel.PaymentTerm = prof.PaymentTerm;
                    vmodel.AIName = prof.AIName;
                    vmodel.AIPhone = prof.AIPhone;
                    vmodel.AIEmail = prof.AIEmail;
                    vmodel.CardHolderEmail = prof.CardEmailAddress;
                    vmodel.CardHolderName = prof.CardContactName;
                    vmodel.CardHolderPhone = prof.CardContactPhone;
                    vmodel.ChkPaymentAddress = (! string.IsNullOrEmpty(prof.AIAddress));
                    vmodel.VendorStatus = prof.VendorStatus;
                    vmodel.AcceptCard = prof.AcceptPurchaseCard == "Y" ? true : false;
                    vmodel.IsCertifiedDiverseFirm = false;
                 
                   
                    Session["vm"] = vmodel;
                }
                else
                {

                    //TempData["Message"] = "Account email already exists. Please make sure you enter different email id.";
                    //Session["vp"] = prof;
                    return RedirectToAction("ThankYou");

                }

            }
            catch (Exception e)
            {

                TempData["Message"] = "An error occured while submitting your details. Please submit it again.Thanks.!!!";
                RedirectToAction("ThankYou");
            }

                return View(vmodel);
        }

        public ActionResult ThankYou()
        {
           // TempData["Message"] = "Your details have been submitted. please contact the Ryan’s Vendor Management team at  <a href="mailto: vendor.mgmt @ryancompanies.com">VendorManagementSupport</a>. Thanks!!!.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult VErrorPage()
        {
            

            return View();
        }






        [HttpPost]
        public async Task<ActionResult> Index(VendorViewModel vm , IEnumerable<HttpPostedFileBase> upload , IEnumerable<HttpPostedFileBase> upload1)
        {
            EventLog v = new EventLog("Application");
            VendorProfile vprof = new VendorProfile();
            v.Source = "Application";
            string fileattach = string.Empty;
            try
            {
                // if model is invlidd
               vm.VendorStatus = "InviteSent";

                if (ModelState.IsValid)
                {
                  //  v.WriteEntry("Model Valid", EventLogEntryType.Information, 101, 1);
                    string devSaveURL = urlpath + "Vendor/SaveVendorProfile/";

                    VendorProfile b = Session["vp"] as VendorProfile;
                    string last = "";
                    string first = "";
                    if (!string.IsNullOrEmpty(vm.PersonCompletingForm) )
                    {
                        string[] names = vm.PersonCompletingForm.Split(new[] { ' ' }, 2);
                        first = names[0];
                        
                        if (names.Length > 1)
                        {
                            last = names[1];
                        }
                    }
                   
                    vprof = b;

                    //ID = b.ID,
                    //JDEVendorID = b.JDEVendorID,
                    //VRVENDORNUMBER = b.VRVENDORNUMBER,
                    //VendorGuid = b.VendorGuid,
                    //IsEinVerified = b.IsEinVerified,
                    //RequestedUserEmail = b.RequestedUserEmail ?? "",
                    // SubmittedTime =  b.SubmittedTime ,
                    //    VendorEmail = b.VendorEmail ?? "",
                    vprof.VendorLegalName = string.IsNullOrEmpty(vm.VendorName) ? vm.VendorDBAName : vm.VendorName; 
                    vprof.VendorStatus = HelperClass.InAccouting;
                    vprof.VendorContactFN = first;
                    vprof.VendorContactLN = last;
                    vprof.VendorDBAName = vm.VendorDBAName ?? "";
                    vprof.VendorEIN = vm.VendorEIN ?? "";
                    vprof.VPhone = vm.VendorPhone ?? "";
                    vprof.VFax = vm.VendorFax ?? "";
                    vprof.VendorCity = vm.VendorCity ?? "";
                    vprof.VendorState = vm.VendorState ?? "";
                    vprof.VendorAddress1 = vm.VendorAddress ?? "";
                    vprof.VendorAddress2 = vm.VendorAddress2 ?? "";
                    vprof.VendorZipCode = vm.VendorZip ?? "";
                    vprof.PersonCorpCode = vm.PersonCorpCode;
                    vprof.PersonSubForm = vm.PersonCompletingForm ?? "";
                    vprof.PersonSubTitle = vm.PersonCompletingTitle ?? "";
                    vprof.UpdatedBy = b.UpdatedBy;
                    vprof.LastUpdatedTime = DateTime.Now;
                    vprof.AIName = vm.AIName ?? "";
                    vprof.AIEmail = vm.AIEmail ?? "";
                    vprof.AIPhone = vm.AIPhone ?? "";
                    vprof.AIState = vm.AIState ?? "";
                    vprof.SignatureSigned = "Y";
                    vprof.AICity = vm.AICity ?? "";
                    vprof.AIAddress = vm.AIAddress ?? "";
                    vprof.AIAddress2 = vm.AIAddress2 ?? "";
                    vprof.AIZip = vm.AIZip ?? "";
                    vprof.PaymentTerm = vm.PaymentTerm ?? "";
                    vprof.StatusChangeReason = b.StatusChangeReason;
                    vprof.STATUSINJDE = string.IsNullOrEmpty(b.STATUSINJDE) ? "NotInJDE" : b.STATUSINJDE;
                    vprof.VendorType = b.VendorType;
                    vprof.AcceptPurchaseCard = vm.AcceptCard ? "Y" : "N";
                    vprof.CardContactName = vm.CardHolderName ?? "";
                    vprof.CardEmailAddress = vm.CardHolderEmail ?? "";
                    vprof.CardContactPhone = vm.CardHolderPhone ?? "";
                    vprof.ProcoreUpdated = "Y" ;

                      
                    // convert to json
                    string obj = JsonConvert.SerializeObject(vprof);
                    var cl = new HttpClient();
                    HttpResponseMessage res = await cl.PostAsync(devSaveURL, new StringContent(obj, Encoding.UTF8, "application/json"));

                    res.EnsureSuccessStatusCode();
                    string responJsonText = await res.Content.ReadAsStringAsync();

                    var json = JObject.Parse(responJsonText);
                    bool iy = false;
                    if (json.ContainsKey("Succeeded"))
                    {
                         iy = Convert.ToBoolean(json["Succeeded"]);
                    }


                    v.WriteEntry("Model saved successfully" + " -- " + vprof.VendorDBAName, EventLogEntryType.Information, 101, 1);

                    if (iy == false)
                    {
                        return RedirectToAction("VErrorPage");
                    }

                    //  string filepath = path + vprof.ID.ToString() + "//";
                    string filepath = filepath1 + "W9//" +  vprof.ID.ToString() + "//";

                    string cdfcfilepath = filepath1 + "CDFC//" + vprof.ID.ToString() + "//";
                    // string s2 = HostingEnvironment.MapPath(filepath);



                    if (!Directory.Exists(filepath))
                    {
                        Directory.CreateDirectory(filepath);
                    }

                    if (!Directory.Exists(cdfcfilepath))
                    {
                        Directory.CreateDirectory(cdfcfilepath);
                    }

                    if ( upload != null)
                    {
                        v.WriteEntry("Saving to " + path + vprof.ID.ToString(), EventLogEntryType.Information, 101, 1);
                        foreach (var file in upload)
                        {
                            if (file is null)
                            {
                                continue;
                            }
                            string sv = filepath1 + "W9\\"  + vprof.ID.ToString() + "\\" + System.IO.Path.GetFileName( file.FileName);
                         
                            file.SaveAs(sv);

                            v.WriteEntry("success : " + file.FileName, EventLogEntryType.Information, 101, 1);
                        }
                    }

                   bool cdfcfile = false;

                    if (upload1 != null)
                    {
                        v.WriteEntry(" Saving CDFC file .. verified", EventLogEntryType.Information, 101, 1);

                        foreach (var file in upload1)
                        {
                          //  v.WriteEntry("CDFC  : " + file.FileName, EventLogEntryType.Information, 101, 1);
                            if ( file is null)
                            {
                                continue;
                            }
                            else
                            {
                                cdfcfile = true;
                            }
                          
                            string sv = filepath1 + "CDFC\\"  + vprof.ID.ToString() + "\\" + System.IO.Path.GetFileName(file.FileName);
                            v.WriteEntry(sv, EventLogEntryType.Information, 101, 1);

                            file.SaveAs(sv);
                             fileattach = sv;

                        }
                    }

                    /// send email to ryan team
                    List<string> emails = new List<string>();
                    //emails.Add(HelperClass.VendorOnBoardingAdminEmail);

                    string AdminEmails = ConfigurationManager.AppSettings["VOBAdminEmail"].ToString();
                    string[] Aemails = AdminEmails.Split(',');
                    foreach (string s in Aemails)
                    {
                        emails.Add(s);
                    }

                    if (!string.IsNullOrEmpty(vprof.RequestedUserEmail))
                    {
                        emails.Add(vprof.RequestedUserEmail);
                    }
                   
                    string emailsub = ConfigurationManager.AppSettings["emailsub"].ToString() + vprof.VendorDBAName;
                    string emailbody =  vprof.VendorDBAName + " has submitted details.";
                    if (cdfcfile)
                    {
                        emailbody = emailbody + " Vendor also submitted a Certified Diverse Firm Certificate for review.";
                    }

                    emailbody = emailbody + " Thanks.";


                    await EmailNotificationUtility.SendEmailToClient(emails, vprof.VendorGuid,vprof.VendorDBAName,  emailbody, emailsub , fileattach);

                    TempData["Message"] = "Your details have been submitted. Please check with support team if you have any questions. Thanks!!!.";
                    return RedirectToAction("ThankYou");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Missing Details");
                    
                    vm.States = LoadStates();
                    vm.PaymentTerms = GetPayementTerms();
                    vm.PersonCorpCodes = GetPersonCorpCodes();
                    return View(vm);
                }
            }
            catch (Exception ex)
            {
                v.WriteEntry("Error Desc : " + ex.InnerException , EventLogEntryType.Error, 101, 1);
                v.WriteEntry("Error Desc : " + ex.Message, EventLogEntryType.Error, 101, 1);
                return RedirectToAction("VErrorPage");
            }
           
 
        }

        //private void SendEmailToRyanUser(VendorProfile vprof)
        //{
        //    EmailNotificationUtility eutility = new EmailNotificationUtility();
        //    eutility.SmtpServer = "Relay.ryancompanies.com";
        //    eutility.From = "Support@RyanCompanies.com";

        //    eutility.Vemail = vprof.RequestedUserEmail;
        //    eutility.Body = string.Format("Hi Team, {0}  vendor submitted details to Ryan companies. To goto vendor details. Please click <a href='http://ryweb16-d:5894/VendorManagement/EditVendor?vid={1}' target='_blank'>here</a>.", vprof.VendorLegalName, vprof.ID.ToString());
        //    eutility.Subject = string.Format(" {0} , submited details to ryan.", vprof.VendorLegalName);
        //    eutility.SendNotification();


        //}

  

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
            new SelectListItem() {Text = "ME", Value = "NE"},
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

        private static IEnumerable<SelectListItem> GetPersonCorpCodes()
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


    }
}