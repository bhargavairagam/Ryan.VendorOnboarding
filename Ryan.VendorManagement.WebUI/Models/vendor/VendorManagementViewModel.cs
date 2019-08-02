using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Ryan.VendorManagement.WebUI.Models.vendor
{
    public class VendorManagementViewModel
    {
      
       

        
        
        public string VendorLegalName { get; set; }

        [Required(ErrorMessage = "Name field required!!")]
       
        public string VendorDBAName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email field required")]
        public string VendorEmail { get; set; }

        [Required(ErrorMessage = "FirstName field required!!")]
     
        public string VContactFirstName { get; set; }

        [Required(ErrorMessage = "LastName field required!!")]
     
        public string VContactLastName { get; set; }

        public string VendorGuid { get; set; }


        [DataType(DataType.MultilineText)]
        public string SpecialInstructions { get; set; }

        public string ID { get; set; }




        public bool MRI1 { get; set; }
        
        public bool MRI2 { get; set; }
        
        public bool MRI10 { get; set; }

        public bool MRI12 { get; set; }


        public bool YARDI5 { get; set; }

        public bool YARDI6 { get; set; }

        public bool YARDI7 { get; set; }

        public bool YARDI8 { get; set; }

        public bool YARDI11 { get; set; }
        public bool YARDI14 { get; set; }

        public bool JDE { get; set; }

        public bool PROCORE { get; set; }

        
    }
}