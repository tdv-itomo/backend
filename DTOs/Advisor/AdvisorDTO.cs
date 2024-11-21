using VicemAPI.DTOs.Student;

namespace VicemAPI.DTOs.Advisor
{
  public class AdvisorDTO
  {
    public int ID { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int DepartmentID { get; set; }

    public DepartmentDTO? Department { get; set; }

    public List<StudentDTO>? Students { get; set; }

    public int CurrentSlot { get; set; } = 0;

    public int MaxSlot { get; set; }
  }
}
