using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ryan.EmployeeOst.Domain.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        public string EmpNumber { get; set; }

        public string EmpType { get; set; }

        public string EmpStatus { get; set; }

        public string First { get; set; }

        public string Middle { get; set; }

        public string Last { get; set; }

        public string NickName { get; set; }

        public string FullName { get; set; }

        public string LfmName { get; set; }

        public string PhoneWork { get; set; }

        public string PhoneCell { get; set; }

        public string PhoneFax { get; set; }

        public string Email { get; set; }

        public string LoginID { get; set; }

        public string JobCode { get; set; }

        public string JobDesc { get; set; }

        public string BusinessCardTitle { get; set; }

        public string JobGrade { get; set; }

        public string HomeBusUnit { get; set; }

        public string HomeBusUnitDesc { get; set; }

        public string OfficeCode { get; set; }

        public string OfficeDesc { get; set; }

        public string RegionCode { get; set; }

        public string RegionDesc { get; set; }

        public string DivisionCode { get; set; }

        public string DivisionDesc { get; set; }

        public string Location { get; set; }

        public string ManagerID { get; set; }

        public string ManagerName { get; set; }

        public string DateHireOrig { get; set; }

        public string DateHireLast { get; set; }

        public string DateTerminated { get; set; }

        public string SSNLast4 { get; set; }

        public string BirthdayMonth { get; set; }

        public string BirthdayDay { get; set; }

        public string Gender { get; set; }

        public string Ethnicity { get; set; }

        public string YrsOfService { get; set; }
    }
}
