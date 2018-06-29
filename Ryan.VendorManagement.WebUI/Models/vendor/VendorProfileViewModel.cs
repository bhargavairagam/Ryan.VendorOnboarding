using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ryan.VendorManagement.WebUI.Models.vendor
{
    public class VendorProfileViewModel
    {
        public int ID { get; set; }
        public string VendorEIN { get; set; }
        public string VendorDBAName { get; set; }
        public string VendorLegalName { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorZipCode { get; set; }
        public string VPhone { get; set; }
        public string VFax { get; set; }
        public string AcceptPurchaseCard { get; set; }
        public string PaymentAddress1 { get; set; }
        public string PaymentAddress2 { get; set; }
        public string PaymentZipCode { get; set; }
        public string CardContactName { get; set; }
        public string CardContactPhone { get; set; }
        public string EmailAddress { get; set; }
        public string VendorStatus { get; set; }
        public string VendorGuid { get; set; }
        public string SignatureSigned { get; set; }
        public string PersonSubForm { get; set; }
        public string PersonSubTitle { get; set; }
    }
}