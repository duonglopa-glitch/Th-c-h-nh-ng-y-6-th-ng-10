using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bai04
{
    internal class Employee
    {
        public string EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public DateTime JoinDate { get; set; }
        public string DepartmentCode { get; set; } 
        public int ImageIndex { get; set; }        

        public Employee(string employeeId, string fullName, string position, DateTime joinDate, string departmentCode, int imageIndex = 0)
        {
            EmployeeId = employeeId;
            FullName = fullName;
            Position = position;
            JoinDate = joinDate;
            DepartmentCode = departmentCode;
            ImageIndex = imageIndex;
        }
    }
}