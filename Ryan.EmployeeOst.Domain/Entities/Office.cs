using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.EmployeeOst.Domain.Entities
{
    [Table("Office")]
    public class Office
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(5)]
        public string OfficeCode { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Addr1 { get; set; }

        [MaxLength(50)]
        public string Addr2 { get; set; }

        [MaxLength(50)]
        public string City { get; set; }

        [MaxLength(5)]
        public string St { get; set; }

        [MaxLength(12)]
        public string Zip { get; set; }

        [MaxLength(5)]
        public string TimeZone { get; set; }

        public string RegionCode { get; set; }

        public string Status { get; set; }

        [MaxLength(5)]
        public string Locality { get; set; }
    }
}
