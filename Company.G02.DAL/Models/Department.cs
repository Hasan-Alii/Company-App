using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.DAL.Models
{
    public class Department : BaseEntity
    {
        [Required(ErrorMessage = "Code Is Required!")]
        [DisplayName("Code")]
        public string Code { get; set; } = "N/A";

        [Required(ErrorMessage = "Name Is Required!")]
        public string Name { get; set; } = "N/A";
        
        [DisplayName("Creation Date")]
        [Required(ErrorMessage = "Creation Date Is Required!")]
        public DateTime DateOfCreation { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
