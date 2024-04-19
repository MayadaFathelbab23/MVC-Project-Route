using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    public enum Gender
    {
        [EnumMember(Value = "Male")] // to store enum in DB as string with value Male
        Male = 1,
        [EnumMember(Value ="Female")]
        Female = 2
    }
    public enum EmployeeType
    {
        [EnumMember(Value ="FullTime")]
        FullTime = 1,
        [EnumMember(Value ="PartTime")]
        PartTime = 2
    }
    public class Employee : ModelBase
    {
       
        public string Name { get; set; }
        public int? Age { get; set; }
    
        public string Address { get; set; }
        public decimal Salary { get; set; }
        
        public bool IsActive { get; set; }
    
        public string Email { get; set; }
       
        public string PhoneNumber { get; set; }
       
        public DateTime HireDate { get; set; }
        public Gender EmployeeGender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public string ImageName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        // Navigational property : [ONE]
        public Department Department { get; set; }
        public int? DepartmentId { get; set; } // FK
    }
}
