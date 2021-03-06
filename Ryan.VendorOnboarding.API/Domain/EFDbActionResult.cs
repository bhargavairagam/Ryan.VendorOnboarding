﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ryan.VendorOnboarding.API.Domain
{
    public class EFDbActionResult : IDbActionResult
    {
        public IEnumerable<string> Errors { get; set; }

        public bool Succeeded
        {
            get
            {
                if (this.Errors.Count() == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public EFDbActionResult()
        {
            Errors = new List<string>();
        }

        public EFDbActionResult(IEnumerable<string> errors)
        {
            Errors = errors;
        }

        public int SavedID { get; set; }
    }
}