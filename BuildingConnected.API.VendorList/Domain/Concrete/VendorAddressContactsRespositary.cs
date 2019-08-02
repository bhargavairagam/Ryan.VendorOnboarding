using BuildingConnected.API.VendorList.Domain.Abstract;
using BuildingConnected.API.VendorList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingConnected.API.VendorList.Domain.Concrete
{
    public class VendorAddressContactsRespositary : IVendorAddressContactsRepositary
    {

        private EFDbContext context = new EFDbContext();

        public VendorAddressContacts GetVendorAddressContacts(string ein)
        {
            VendorAddressContacts v = new VendorAddressContacts();
            
            v = context.VendorAddressContacts.Where(a => a.VendorEIN == ein).FirstOrDefault();

            return v;
        }

        public async Task<IEnumerable<VendorAddressContacts>>  GetAllVendorAddressContacts()
        {
            VendorAddressContacts v = new VendorAddressContacts();
            return await Task<IEnumerable<VendorAddressContacts>>.Factory.StartNew(() =>
            {
                return context.VendorAddressContacts.ToList(); ;
            });
           
        }

        public IDbActionResult SaveVendorAddressContacts(VendorAddressContacts vendorAddCont)
        {
            EFDbActionResult results = new EFDbActionResult();
            List<string> errors = new List<string>();


            context.VendorAddressContacts.AddOrUpdate(vendorAddCont);
            results.SavedID = context.SaveChanges();

            return results;
        }


        public async Task<IDbActionResult> SaveVendorAddressContactsAsync(VendorAddressContacts vendorAddCont)
        {
            EFDbActionResult results = new EFDbActionResult();

                context.VendorAddressContacts.AddOrUpdate(vendorAddCont);
                results.SavedID = context.SaveChanges();

            return await Task<IDbActionResult>.Factory.StartNew(() =>
            {
                return results;
            });


            //return results;
        }
    }
}
