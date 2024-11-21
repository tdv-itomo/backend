using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VicemAPI.Models.Entities
{
  public class Company
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    public string Name { get; set; }

    public string Field { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = "Chờ duyệt";
  }
}