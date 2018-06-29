using Ryan.VendorOnboarding.API.Domain.Entities;
using Ryan.VendorOnboarding.API.Domain.VendorDetails;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using Ryan.VendorOnboarding.API.Infrastructure;

namespace Ryan.VendorOnboarding.API.Domain.Concrete
{
    public class EFVendorDetailsRepositary : IVendorDetailsRepositary
    {
        private EFDbContext context = new EFDbContext();

        public async Task<VendorProfile> GetVendorDetials(string ein)
        {

            return await Task<VendorProfile>.Factory.StartNew(() =>
            {
                return context.VendorProfiles.Where(a => a.VendorEIN == ein).FirstOrDefault();
            });

        }

        public VendorProfile GetVendorDetials1(string ein)
        {

           
                return context.VendorProfiles.Where(a => a.VendorEIN == ein).FirstOrDefault();
          

        }




        public async Task<VendorProfile> GetVendorDetialsByGuid(string guid)
        {

            return await Task<VendorProfile>.Factory.StartNew(() =>
            {
                return context.VendorProfiles.Where(a => a.VendorGuid == guid).FirstOrDefault();
            });

        }

        public async Task<IDbActionResult> SaveVendorDetials(VendorProfile vendorProfile)
        {
            EFDbActionResult results = new EFDbActionResult();
            List<string> errors = new List<string>();
           
            try
            {
                context.VendorProfiles.AddOrUpdate(vendorProfile);
                results.SavedID = context.SaveChanges();
            }
            catch(Exception ex) {
                errors.Add("Cannot find custompage to update");
                errors.AddRange(ErrorHelper.GetAllExceptionMessages(ex));
            }
            results.Errors = errors;


            return await Task<IDbActionResult>.Factory.StartNew(() =>
            {
                return results;
            });

           
        }

        public async Task<IEnumerable<VendorProfile>> GetVendorProfiles()
        {
            return await Task<IEnumerable<VendorProfile>>.Factory.StartNew(() =>
            {
                return context.VendorProfiles.ToList();
            });
        }
    }
}