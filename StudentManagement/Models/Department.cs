using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required]
        public string DepartmentName { get; set; }
        
        // Navigation collections
        public ICollection<Course> Courses { get; set; } = new List<Course>();
        public ICollection<Faculty> Faculties { get; set; } = new List<Faculty>();
    }
}