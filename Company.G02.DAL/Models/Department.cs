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
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "CODE IS REQUIRED !!")]
        [DisplayName("# Code")]
        public string Code { get; set; } = "N/A";

        [Required(ErrorMessage = "NAME IS REQUIRED !!")]
        [DisplayName("Title")]
        public string Name { get; set; } = "N/A";
        
        [DisplayName("Date of Creation")]
        public DateTime DateOfCreation { get; set; } 
    }
}
