using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Employee_Creation.Models
{
    public class Employee
    {
        
    public int ID { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string PicturePath { get; set; } // Path to the image file
    }
}
