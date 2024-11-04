using System.ComponentModel.DataAnnotations;

namespace VicemAPI.Models.Entities
{
  public class Student
  {
    [Key]
    public int ID { get; set; }
    public string StudentId { get; set; }
    [Required]
    public string Name { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Class { get; set; }
    [Required]
    public string Course { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}