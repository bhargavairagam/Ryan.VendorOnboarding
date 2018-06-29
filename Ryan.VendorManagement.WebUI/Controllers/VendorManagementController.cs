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
using Ryan.Vendorboarding.Domain;

namespace Ryan.VendorManagement.WebUI.Controllers
{
    public class VendorManagementController : Controller
    {
        EFVendorProfileRepositary vendorProfileRepository = new EFVendorProfileRepositary();


        // GET: VendorManagement
        public async Task<ActionResult> Index()
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

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
                vendorProfiles.Add(v);

            }


            return View(vendorProfiles);
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
                vendorProfiles.Add(v);

            }


            return View(vendorProfiles);
        }


        public async Task<ActionResult> GetVendorDetails(String EIN)
        {

            List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

            var pro = profiles.FirstOrDefault(x => x.VendorEIN == EIN);

            VendorProfileViewModel v = new VendorProfileViewModel
            {
                VendorLegalName = pro.VendorLegalName,
                VendorEIN = pro.VendorEIN,
                VendorZipCode = pro.VendorZipCode,
                VFax = pro.VFax,
                VPhone = pro.VPhone,
                VendorDBAName = pro.VendorDBAName,
                EmailAddress = pro.CardEmailAddress,
                 VendorStatus = pro.VendorStatus,
                 VendorAddress1 = pro.VendorAddress1,
                 VendorAddress2 = pro.VendorAddress2

            };


            return View("VendorDetails",v);
        }


        [HttpGet]
        public  ActionResult AddVendor()
        {
            return View();
        }



        [HttpPost]
        public async Task<ActionResult> AddVendor(VendorManagementViewModel vend)
        {
            if (ModelState.IsValid)
            {
                // check if email exists
                IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

                var pro = profiles.FirstOrDefault(x => x.VendorEIN == vend.VendorEmail);

                if(pro == null)
                {
                    TempData["AlertMessage"] = "Account email already exists. Please make sure you enter different email id.";   
                    return View(vend);
                }

                VendorProfile vprofile = new VendorProfile();
                Guid vguid = Guid.NewGuid();
                vprofile.VendorLegalName = vend.VendorLegalName;
                vprofile.VendorEIN = vend.VendorEIN;
                vprofile.CardEmailAddress = vend.VendorEmail;
                vprofile.VendorGuid = vguid.ToString();
                vprofile.VendorStatus = "EmailSent";

                IDbActionResult saveResult = await vendorProfileRepository.SaveVendorDetials(vprofile);


                // send email to vendor
                EmailNotificationUtility eutility = new EmailNotificationUtility();
                eutility.SmtpServer = "Relay.ryancompanies.com";
                eutility.From = "Support@RyanCompanies.com";
                

                eutility.vemail = vend.VendorEmail;
                eutility.Body = string.Format("Hi {0} Vendor , Please click below link and submit your details to Ryan companies.: <a href='http://localhost/Vendor/VendorDetails?vendorID={1}' target='_blank'>Submit Details</a>.", vend.VendorLegalName, vguid.ToString());
                eutility.Subject = string.Format(" {0} , welcome to ryan.", vend.VendorLegalName );
                eutility.SendNotification();

                return RedirectToAction("Index");
            }
            else
            {
                TempData["AlertMessage"] = "There was an error saving the override, please try again.";
                return View(vend);
            }




        }
    }
}