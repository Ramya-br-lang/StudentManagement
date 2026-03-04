using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentManagementSystem.Models
{
    public class FacultyCourse
    {
        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int CourseId { get; set; }

        // Navigation properties
        [ForeignKey("FacultyId")]
        public virtual Faculty? Faculty { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }
    }
}
