using Company.G02.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace Company.G02.PL.ViewModels.Employee
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Range(25, 60, ErrorMessage = "Age Must Be Between 25 - 60")]
        public int? Age { get; set; }

        [RegularExpression(@"[0-9]{0,3}-[a-zA-Z]{2,10}-[a-zA-Z]{2,10}-[a-zA-Z]{2,10}$",
            ErrorMessage = "Address Must Be Like This Formula: 123-Street-City-Country")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Salary Is Required !!")]

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        //[DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; }
        public int? WorkForId { get; set; }
        public Department? WorkFor { get; set; } // Navigational Property
    }
}
