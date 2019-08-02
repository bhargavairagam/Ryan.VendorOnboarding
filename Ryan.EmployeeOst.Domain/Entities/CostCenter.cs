using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.EmployeeOst.Domain.Entities
{
    public class CostCenter
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string CombinedValue { get; set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public CostCenter()
        {
            Code = string.Empty;
            Description = string.Empty;
            CombinedValue = string.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        public CostCenter(string code, string description)
        {
            Code = code;
            Description = description;
            CombinedValue = code + " - " + description;
        }
    }
}
