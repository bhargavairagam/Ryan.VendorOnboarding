using BuildingConnected.API.VendorList.Domain.Abstract;
using BuildingConnected.API.VendorList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingConnected.API.VendorList.Domain.Concrete
{
    public class VendorProfileRepositary:IVendorProfileRepositary
    {
        private EFDbContext context = new EFDbContext();


        public IEnumerable<VendorProfile> GetAllVendorDetails()
        {

            IEnumerable<VendorProfile> res = context.VendorProfiles.Distinct().ToList();
            return res;

        }


        public VendorProfile GetVendorDetials(string ein)
        {

            VendorProfile v = new VendorProfile();
            try
            {
                v = context.VendorProfiles.Where(a => a.VendorEIN == ein).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return v;

        }

        public IDbActionResult SaveVendorDetials(VendorProfile vendorProfile)
        {
            EFDbActionResult results = new EFDbActionResult();
            List<string> errors = new List<string>();

            try
            {
                vendorProfile.LastUpdatedTime = DateTime.Now;
                context.VendorProfiles.AddOrUpdate(vendorProfile);
                results.SavedID = context.SaveChanges();
            }
            catch (Exception ex)
            {
                errors.Add("Cannot find custompage to update");
                // errors.AddRange(ErrorHelper.GetAllExceptionMessages(ex));
            }
            results.Errors = errors;


            
            return results;
            
        }


        public async Task<EFDbActionResult> SaveVendorProfile(VendorProfile vendorProfile)
        {
            EFDbActionResult results = new EFDbActionResult();
            List<string> errors = new List<string>();

            
            vendorProfile.LastUpdatedTime = DateTime.Now;
            context.VendorProfiles.AddOrUpdate(vendorProfile);
            results.SavedID = context.SaveChanges();
            results.Errors = errors;


            return await Task<EFDbActionResult>.Factory.StartNew(() =>
            {
                return results;
            });


        }
    }
}
