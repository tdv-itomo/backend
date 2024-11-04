using System.ComponentModel.DataAnnotations;

namespace VicemAPI.Models.Entities
{
  public class Department
  {
    [Key]
    public int ID { get; set; }
    [Required]
    public int DepartmentId { get; set; }
    [Required]
    public string DepartmentName { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Address { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}