﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuildingConnected.API.VendorList.Domain.Entities
{
    [Table("VendorAddressContacts")]
    public class VendorAddressContacts
    {
        [Key]
        public int ID { get; set; }

        public string VendorName { get; set; }

        public string VendorEIN { get; set; }

        public string VendorAddress { get; set; }
        public string VendorPhone { get; set; }


        public string City { get; set; }
        public string Stat { get; set; }
        public string StNumber { get; set; }
        public string StName { get; set; }

        public string Zip { get; set; }
       
        public string CoordsLong { get; set; }
        public string CoordsLat { get; set; }

        public string VendBusiness1 { get; set;}
        public string VendBusiness2 { get; set; }

        public string BiddingCount { get; set; }
        public string ViewedPercent { get; set; }
        public string ViewedCount { get; set; }
        public string InviteCount { get; set; }
        

        public string AwardedCount { get; set; }
        public string BidPercent { get; set; }
        public string BidCount { get; set; }
        public string BiddingPercent { get; set; }

        public string AwardedPercent { get; set; }
        public string DeclinedCount { get; set; }
        public string DeclinedPercent { get; set; }
        public string Website { get; set; }
        public string BusinessType { get; set; }
        public string VendorAverageRating { get; set; }

        public string FirstContactName { get; set; }
        public string FirstContactEmail { get; set; }
        public string FirstContactPhone { get; set; }

        public string SecondContactName { get; set; }
        public string SecondContactEmail { get; set; }
        public string SecondContactPhone { get; set; }

        public string ThirdContactName { get; set; }
        public string ThirdContactEmail { get; set; }
        public string ThirdContactPhone { get; set; }

        public string FourthContactName { get; set; }
        public string FourthContactEmail { get; set; }
        public string FourthContactPhone { get; set; }


        public string FifthContactName { get; set; }
        public string FifthContactEmail { get; set; }
        public string FifthContactPhone { get; set; }

        public string JDEVendorID { get; set; }
        public string SourceType { get; set; }

    }
}
