using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.EmployeeOst.Domain.Entities
{
    [Table("Region")]
    public class Region
    {
        [Key]
        public int ID { get; set; }

        public string RegionCode { get; set; }

        public string Name { get; set; }

        public string TimeZone { get; set; }

        public string Status { get; set; }
    }
}
