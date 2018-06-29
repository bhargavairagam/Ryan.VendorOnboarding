using System;
using System.Collections.Generic;
using System.Text;
using Ryan.VendorOnboarding.Domain.Abstract;
using Ryan.VendorOnboarding.Domain.Entities;
using System.Threading.Tasks;
using Ryan.Vendorboarding.Domain;
using System.Linq;
using System.Data.Entity.Migrations;

namespace Ryan.VendorOnboarding.Domain.Concrete
{
    public class EFVendorProfileRepositary : IVendorProfileRepositary
    {
        private EFDbContext context = new EFDbContext();

        

        public async Task<IEnumerable<VendorProfile>> GetVendorProfiles()
        {

            return await Task<IEnumerable<VendorProfile>>.Factory.StartNew(() =>
            {
                return context.VendorProfiles.ToList();
            });

        }


        public async Task<VendorProfile> GetVendorDetials(string ein)
        {

            return await Task<VendorProfile>.Factory.StartNew(() =>
            {
                return context.VendorProfiles.Where(a => a.VendorEIN == ein).FirstOrDefault();
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
            catch (Exception ex)
            {
                errors.Add("Cannot find custompage to update");
               // errors.AddRange(ErrorHelper.GetAllExceptionMessages(ex));
            }
            results.Errors = errors;


            return await Task<IDbActionResult>.Factory.StartNew(() =>
            {
                return results;
            });


        }

        

    }
}
