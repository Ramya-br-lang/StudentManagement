using System.ComponentModel.DataAnnotations;

namespace StudentManagementSystem.Models
{
    public class Faculty
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Designation { get; set; }

        // Faculty must belong to a Department
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
