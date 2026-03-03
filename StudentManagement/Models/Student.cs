using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string? EmailId { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int Age { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department? Department { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Course is required")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        public DateTime CourseStartDate { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        public DateTime CourseEndDate { get; set; }
    }
}