using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.EmployeeOst.Domain.Entities
{
    public class Job
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string CombinedValue { get; set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public Job()
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
        public Job(string code, string description)
        {
            Code = code;
            Description = description;
            if (string.IsNullOrEmpty(code))
            {
                if (string.IsNullOrEmpty(description))
                {
                    CombinedValue = string.Empty;
                }
                else
                {
                    CombinedValue = description;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(description))
                {
                    CombinedValue = code;
                }
                else
                {
                    CombinedValue = code + " - " + description;
                }
            }
            
        }
    }
}
