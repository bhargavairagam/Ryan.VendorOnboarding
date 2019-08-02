using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ryan.VendorOnboarding.WebUI.Models
{
    public class VendorViewModel
    {
        public VendorViewModel()
        {
            Files = new List<HttpPostedFileBase>();
            
        }

        public List<HttpPostedFileBase> Files { get; set; }

       

        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> PaymentTerms { get; set; }
        public IEnumerable<SelectListItem> PersonCorpCodes { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Vendor Name required")]
        public string VendorName { get; set; }

        [MaxLength(50)]
        public string CircleOne { get; set; }

        
        [Required(ErrorMessage = "EIN/SSN required")]
        public string VendorEIN { get; set; }

        
        [Required(ErrorMessage ="DBA Name required")]
        public string VendorDBAName { get; set; }

        [MaxLength(50)]
        public string AddressState { get; set; }

        [MaxLength(50)]
        public string VendorLastName { get; set; }

        [MaxLength(50)]
        public string VendorAddress { get; set; }

        [MaxLength(50)]
        public string VendorAddress2 { get; set; }

        [MaxLength(50)]
        public string VendorState { get; set; }

        [MaxLength(50)]
        public string VendorCity { get; set; }

        public bool ChkPaymentAddress { get; set; }


        public string VendorZip { get; set; }

        public string VendorPhone { get; set; }

        [MaxLength(50)]
        public string VendorFax { get; set; }

        [MaxLength(50)]
        public string VendorEmail { get; set; }

        public bool AcceptCard { get; set; }

        [MaxLength(100)]
        public string CardHolderName { get; set; }

        [MaxLength(50)]
        [Phone]
        public string CardHolderPhone { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CardHolderEmail { get; set; }

        
        public bool IsCertifiedDiverseFirm { get; set; }

        [MaxLength(100)]
        [Required(ErrorMessage = "Person Name")]
        public string PersonCompletingForm { get; set; }

        
        [Required(ErrorMessage = "Person Title")]
        public string PersonCompletingTitle { get; set; }

        

        [MaxLength(50)]
        public string AIName{ get; set; }

        public string AIPhone { get; set; }

        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string AIEmail { get; set; }

        [MaxLength(200)]
        public string AIAddress { get; set; }
        [MaxLength(100)]
        public string AIAddress2 { get; set; }
        [MaxLength(200)]
        public string AICity { get; set; }
        [MaxLength(50)]
        public string AIState { get; set; }
        [MaxLength(50)]
        public string AIZip { get; set; }

        [MaxLength(50)]
        public string PaymentTerm { get; set; }

        [MaxLength(50)]
        public string PersonCorpCode { get; set; }
        public string VendorStatus { get; set; }


    }


    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }






}