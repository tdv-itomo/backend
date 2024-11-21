using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
    public class Advisor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Khóa ngoại DepartmentID
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public ICollection<Student> Students { get; set; }

        public ICollection<Council>? Councils { get; set; }

        public int CurrentSlot { get; set; } = 0;

        public int MaxSlot { get; set; }

        // Navigation property
        public Department? Department { get; set; }
    }
}