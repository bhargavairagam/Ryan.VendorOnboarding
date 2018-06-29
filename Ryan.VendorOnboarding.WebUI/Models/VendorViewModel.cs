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

        [MaxLength(50)]
        public string VendorName { get; set; }

        [MaxLength(50)]
        public string CircleOne { get; set; }

        [MaxLength(10)]
        public string VendorEIN { get; set; }

        [MaxLength(25)]
        public string VendorDBAName { get; set; }

        [MaxLength(50)]
        public string AddressState { get; set; }

        [MaxLength(50)]
        public string VendorLastName { get; set; }

        [MaxLength(50)]
        public string VendorAddress { get; set; }

        [MaxLength(5)]
        public string VendorZip { get; set; }


        [MaxLength(10)]
        public string VendorPhone { get; set; }

        [MaxLength(50)]
        public string VendorFax { get; set; }

        public bool AcceptCard { get; set; }

        [MaxLength(50)]
        public string CardHolderName { get; set; }

        [MaxLength(50)]
        public string CardHolderPhone { get; set; }

        [MaxLength(50)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string CardHolderEmail { get; set; }

        [Required(ErrorMessage = "Please check acknowledgement!!")]
        public bool AcceptAboveDetails { get; set; }

        [MaxLength(50)]
        public string PersonCompletingForm { get; set; }

        [MaxLength(20)]
        public string PersonCompletingTitle { get; set; }


    }


   



}