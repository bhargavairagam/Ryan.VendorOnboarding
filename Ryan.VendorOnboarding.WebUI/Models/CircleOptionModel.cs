using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Ryan.VendorOnboarding.WebUI.Models
{
    public class CircleOptionModel
    {
        public CircleOptionModel(string key, string value)
        {
            CircleOptionKey = key;
            CircleOptionValue = value;

        }

        public CircleOptionModel() { }
        public string CircleOptionKey { get; set; }
        public string CircleOptionValue { get; set; }

     
    }
}