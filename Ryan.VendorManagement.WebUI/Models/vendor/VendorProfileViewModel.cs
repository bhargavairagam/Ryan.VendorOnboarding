using Ryan.VendorOnboarding.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Ryan.VendorManagement.WebUI.Infrastructure;
using System.Web.Mvc;

namespace Ryan.VendorManagement.WebUI.Models.vendor
{
    public class VendorProfileViewModel
    {

        public IEnumerable<SelectListItem> Statuses { get; set; }

        public List<string> Folderfiles { get; set; }
        public List<FileDetails> UploadfilesList { get; set; }

        public int ID { get; set; }
        public string VendorEIN { get; set; }

        [MaxLength(40, ErrorMessage = "DBA Name is more than 40 characters.")]
        [Required(ErrorMessage = "DBA required")]
        public string VendorDBAName { get; set; }

        [MaxLength(40, ErrorMessage = "Legal Name is more than 40 characters.")]
        public string VendorLegalName { get; set; }

        [MaxLength(40, ErrorMessage = "Address should not be more than 40 characters.")]
        public string VendorAddress1 { get; set; }
        [MaxLength(40, ErrorMessage = "Address should not be more than 40 characters.")]
        public string VendorAddress2 { get; set; }

        public string VendorState { get; set; }

        [MaxLength(25, ErrorMessage = "City should not be more than 25 characters.")]
        public string VendorCity{ get; set; }

      
      
        public string VendorZipCode { get; set; }

        
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]  
        public string VendorEmail { get; set; }

        public string JDEStatus { get; set; }

        public IEnumerable<SelectListItem> JDEStatuses { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        public IEnumerable<SelectListItem> PaymentTerms { get; set; }
        public IEnumerable<SelectListItem> PersonCorpCodes { get; set; }
        public string PersonCorpCode { get; set; }
        public string PaymentTerm { get; set; }

  
        [MinLength(12, ErrorMessage = "Incomplete Phonenumber.")]
        [MaxLength(12, ErrorMessage = "Incomplete Phonenumber.")]
        public string VPhone { get; set; }
        public string VFax { get; set; }
        public string AcceptPurchaseCard { get; set; }
        public string PaymentAddress1 { get; set; }
        public string PaymentAddress2 { get; set; }

        
        public string PaymentZipCode { get; set; }
        public string CardContactName { get; set; }
        public string CardContactPhone { get; set; }
        public string CardContactEmail { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }
        public string VendorStatus { get; set; }
        [MaxLength(50)]
        public string VendorGuid { get; set; }
        [MaxLength(50)]
        public string SignatureSigned { get; set; }

        [MaxLength(100, ErrorMessage = "Name is more than 100 characters.")]
        public string PersonSubForm { get; set; }

        
        public string PersonSubTitle { get; set; }

        public string RequestedUserEmail { get; set; }
        public bool IsEINValid { get; set; }

        [MaxLength(50)]
        [RegularExpression(@"^[0-9a-zA-Z'''\s]{1,40}$", ErrorMessage = "No special charatcers allowed")]
        public string VContactFirstName { get; set; }
        
        [MaxLength(50)]
        [RegularExpression(@"^[0-9a-zA-Z'''\s]{1,40}$", ErrorMessage = "No special charatcers allowed")]
        public string VContactLastName { get; set; }

        [MaxLength(50)]
        public string JDEVendorID { get; set; }

        public IEnumerable<StatusHistory> StatusHistoryList { get; set; }


        [MaxLength(100)]
        [RegularExpression(@"^[0-9a-zA-Z'''\s]{1,40}$", ErrorMessage = "No special charatcers allowed")]
        public string AIName { get; set; }

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


        [DataType(DataType.MultilineText)]
        public string StatusChangeReason { get; set; }

        [MaxLength(50)]
        public string VendorType { get; set; }

        public bool MRI1 { get; set; }
        
        public bool MRI2 { get; set; }
        
        public bool MRI10 { get; set; }
        
        public bool MRI12 { get; set; }
        public bool YARDI5 { get; set; }
        
        public bool YARDI14 { get; set; }
        
        public bool YARDI7 { get; set; }
        
        public bool YARDI8 { get; set; }

        public bool YARDI11 { get; set; }
       

        public bool JDE { get; set; }
        
        public bool PROCORE { get; set; }

        public bool IsCertifiedDiverseFirm { get; set; }

        // package info
        public string MRI1Code { get; set; }
        public string MRI2Code { get; set; }
        public string MRI12Code { get; set; }
        public string YARDI5Code { get; set; }
        public string YARDI7Code { get; set; }
        public string YARDI8Code { get; set; }
        public string YARDI11Code { get; set; }
        public string YARDI14Code { get; set; }

        public bool AcceptCard { get; set; }

        public string SourceType { get; set; }



    }
}