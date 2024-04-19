using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name must be max 50 Char")]
        [MinLength(5, ErrorMessage = "Name must me at least 5 chars")]
        public string Name { get; set; }
        [Range(22, 30)]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        public IFormFile Image { get; set; } // to upload from html form
        public string ImageName { get; set; } // to mapp it in model
        public Gender EmployeeGender { get; set; }
        public EmployeeType EmployeeType { get; set; }
        
        // Navigational property : [ONE]
        //[InverseProperty(nameof(Models.Department.Employees))]
        public Department Department { get; set; }
        //[ForeignKey(nameof(Department))]
        public int? DepartmentId { get; set; } // FK
        // Optional FK = Restrict delete rule
        // Required FK = Cascade Delete rule
    }
}
