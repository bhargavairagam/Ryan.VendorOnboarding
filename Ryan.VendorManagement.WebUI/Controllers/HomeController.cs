﻿
using System.Web.Mvc;
using Ryan.VendorManagement.WebUI.Models.vendor;
using Ryan.VendorOnboarding.Domain.Entities;
using Ryan.VendorOnboarding.Domain.Concrete;
using System.Threading.Tasks;

namespace Ryan.VendorManagement.WebUI.Controllers
{
    public class HomeController : Controller
    {
        EFVendorProfileRepositary vendorProfileRepository = new EFVendorProfileRepositary();

        public ActionResult Index()
        {

            //List<VendorProfileViewModel> vendorProfiles = new List<VendorProfileViewModel>();
            //IEnumerable<VendorProfile> profiles = await vendorProfileRepository.GetVendorProfiles();

            //foreach (VendorProfile pro in profiles)
            //{
            //    VendorProfileViewModel v = new VendorProfileViewModel();
            //    v.AcceptPurchaseCard = pro.AcceptPurchaseCard;
            //    v.CardContactName = pro.CardContactName;
            //    v.CardContactPhone = pro.CardContactPhone;
            //    v.EmailAddress = pro.CardEmailAddress;
            //    v.VendorStatus = pro.VendorStatus;
            //}


            return RedirectToAction("Index","VendorManagement");
        }

     
    }
}