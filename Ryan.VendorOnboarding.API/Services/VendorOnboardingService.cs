
using System.Threading.Tasks;
using Ryan.VendorOnboarding.API.Domain.Concrete;
using Ryan.VendorOnboarding.API.Domain.Entities;
using Ryan.VendorOnboarding.API.Domain;
using System;

namespace Ryan.VendorOnboarding.API.Services
{
    public class VendorOnboardingService
    {
        private EFVendorDetailsRepositary vendorRepo;

        public VendorOnboardingService()
        {
            vendorRepo = new EFVendorDetailsRepositary();
        }

        public async Task<VendorProfile> GetVendorDetails(string einNum)
        {
            VendorProfile vdetails = new VendorProfile();

            try
            {
                vdetails = await vendorRepo.GetVendorDetials(einNum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

            return vdetails;
        }


        public VendorProfile GetVendorDetails1(string einNum)
        {
            VendorProfile vdetails = new VendorProfile();
            EFDbContext context = new EFDbContext();

            vdetails = vendorRepo.GetVendorDetials1(einNum);

            return vdetails;
        }

        public async Task<VendorProfile> GetVendorDetailsByGuid(string guid)
        {
            VendorProfile vdetails = new VendorProfile();

            vdetails = await vendorRepo.GetVendorDetialsByGuid(guid);

            return vdetails;
        }



        public async Task<bool> SaveVendorDetails(VendorProfile vdetails)
        {

            IDbActionResult res = new EFDbActionResult(); 
            // check if profile already exists
            VendorProfile vdet = await vendorRepo.GetVendorDetials(vdetails.VendorEIN);
            if(vdet.VendorEIN != "")
            {
                res = await vendorRepo.SaveVendorDetials(vdetails);
            }
             

            return res.Succeeded;
        }

       
    }
}