using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
  public class Topic
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Name { get; set; }

    public string Status { get; set; } = "Chờ duyệt đề tài";

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Student Student { get; set; }
  }
}