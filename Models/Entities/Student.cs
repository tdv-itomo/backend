using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(20)]
        public string Code { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(10)]
        public string Class { get; set; }

        [Required]
        [StringLength(10)]
        public string Course { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public Department? Department { get; set; }

        [ForeignKey("Advisor")]
        public int? AdvisorID { get; set; }

        public Advisor? Advisor { get; set; }

        [ForeignKey("Council")]
        public int? CouncilID { get; set; }

        public Council? Council { get; set; }

        [ForeignKey("Topic")]
        public int? TopicID { get; set; }

        public Topic? Topic { get; set; }

        [ForeignKey("Company")]
        public int? CompanyID { get; set; }

        public Company? Company { get; set; }

        public string Status { get; set; } = "Mới";

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public float? FirstScore { get; set; }

        public float? SecondScore { get; set; }

        public float? ThirdScore { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}