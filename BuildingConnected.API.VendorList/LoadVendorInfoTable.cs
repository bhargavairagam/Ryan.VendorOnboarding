using BuildingConnected.API.VendorList.Domain.Concrete;
using BuildingConnected.API.VendorList.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingConnected.API.VendorList
{
    public static class LoadVendorInfoTable
    {
        public static async Task<bool> LoadVendorInfoFromMaster()
        {
            VendorAddressContactsRespositary varep = new VendorAddressContactsRespositary();
            VendorProfileRepositary vprofrep = new VendorProfileRepositary();

            var vendorcontacts = await varep.GetAllVendorAddressContacts();

            int i = 0;
            foreach(VendorAddressContacts ven in vendorcontacts)
            {
                i++;
                bool suc = true;
                VendorProfile vprof = new VendorProfile();
                vprof.AcceptPurchaseCard = "N";
                vprof.IsEinVerified = "N";
                vprof.LastUpdatedTime = DateTime.Now;
                vprof.UpdatedBy = "biragam;";
                // load address
                vprof.VendorAddress1 = ven.StName;
                vprof.VendorAddress2 = ven.StNumber;
                vprof.VendorCity = ven.City;
                vprof.VendorState = ven.Stat;
                vprof.VendorZipCode = ven.Zip;
                vprof.VPhone = ven.VendorPhone;

                vprof.VendorStatus = "Approved";
                vprof.VendorContactFN = ven.FirstContactName;
                vprof.VendorEmail = ven.FirstContactEmail;
                
                vprof.VendorContactLN = string.Empty;
                vprof.VendorDbaName = string.Empty;
                vprof.VendorEIN = ven.VendorEIN;
                vprof.VendorGuid = Guid.NewGuid().ToString();
                vprof.VendorLegalName = ven.VendorName;
                vprof.VFax = string.Empty;

                // Authorized individuals
                vprof.AIAddress = string.Empty;
                vprof.AIAddress2 = string.Empty;
                vprof.AICity = string.Empty;
                vprof.AIEmail = string.Empty;
                vprof.AIName = string.Empty;
                vprof.AIPhone = string.Empty;
                vprof.AIZip = string.Empty;

                vprof.JDEVendorID = ven.JDEVendorID;
                vprof.SourceType = ven.SourceType;

                Console.WriteLine(" {0} , VendorName : {1}", i, vprof.VendorLegalName);
                try
                {
                    var x = await vprofrep.SaveVendorProfile(vprof);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(" {0} , VendorName : {1}", i, vprof.VendorLegalName);
                }
            

                
              

            }


            return true;
        }
    }
}
