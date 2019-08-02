
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

            // check status of vendor. Get only submitted status vendors


            vdetails = await vendorRepo.GetVendorDetialsByGuid(guid);

            return vdetails;
        }

        public async Task<VendorProfile> GetVendorDetialsByGuidAndStatus(string guid,string status)
        {
            VendorProfile vdetails = new VendorProfile();

            // check status of vendor. Get only submitted status vendors
            vdetails = await vendorRepo.GetVendorDetialsByGuidAndStatus(guid, status);

            return vdetails;
        }



        public async Task<IDbActionResult> SaveVendorDetails(VendorProfile vdetails)
        {

            IDbActionResult res = new EFDbActionResult();
            if(vdetails.ID > 0)
            {
                vdetails.LastUpdatedTime = DateTime.Now;
            }
            res = await vendorRepo.SaveVendorDetials(vdetails);
            return res;
        }
        
       
    }
}