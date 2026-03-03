using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class FacultyCourse
    {
        public int Id { get; set; }

        public int FacultyId { get; set; }
        [ForeignKey("FacultyId")]
        public Faculty? Faculty { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}
