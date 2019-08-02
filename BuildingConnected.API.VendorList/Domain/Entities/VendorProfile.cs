using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingConnected.API.VendorList.Domain.Entities
{
    [Table("VendorProfileNew")]
    public class VendorProfile
    {

        [Key]
        public int ID { get; set; }
        public string VendorEIN { get; set; }
        public string VendorDbaName { get; set; }
        public string VendorLegalName { get; set; }
        public string VendorAddress1 { get; set; }
        public string VendorAddress2 { get; set; }
        public string VendorZipCode { get; set; }
        public string VPhone { get; set; }
        public string VFax { get; set; }
        public string VendorEmail { get; set; }
        public string VendorState { get; set; }
        public string VendorCity { get; set; }

        public string AcceptPurchaseCard { get; set; }
        public string PaymentAddress1 { get; set; }
        public string PaymentAddress2 { get; set; }
        public string PaymentZipCode { get; set; }

        public string CardContactName { get; set; }
        public string CardContactPhone { get; set; }
        public string CardEmailAddress { get; set; }

        public string SignatureSigned { get; set; }
        public string VendorStatus { get; set; }

        public string VendorGuid { get; set; }
        public string PersonSubForm { get; set; }
        public string PersonSubTitle { get; set; }
        

        public string VendorContactFN { get; set; }
        public string VendorContactLN { get; set; }

        public string UpdatedBy { get; set; }
        public string IsEinVerified { get; set; }

        public DateTime? LastUpdatedTime { get; set; }


        public string AIName { get; set; }
        public string AIPhone { get; set; }
        public string AIEmail { get; set; }

        public string AIAddress2 { get; set; }
        public string AIAddress { get; set; }
        public string AICity { get; set; }
        public string AIState { get; set; }
        public string AIZip { get; set; }
        public string RequestedUserEmail { get; set; }
        public string JDEVendorID { get; set; }
        public string SourceType { get; set; }


    }
}
