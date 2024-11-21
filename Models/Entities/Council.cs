using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
  public class Council
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime ReviewTime { get; set; }

    public string Location { get; set; }

    public int DepartmentID { get; set; }

    public Department Department { get; set; }

    public ICollection<Student> Students { get; set; }

    public ICollection<Advisor> Advisors { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
  }
}