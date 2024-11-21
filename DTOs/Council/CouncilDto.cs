using VicemAPI.DTOs.Advisor;
using VicemAPI.DTOs.Student;

namespace VicemAPI.DTOs.Council
{
  public class CouncilDto
  {
    public int ID { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public DateTime ReviewTime { get; set; }

    public string Location { get; set; }

    public int DepartmentID { get; set; }

    public DepartmentDTO Department { get; set; }

    public List<StudentSimpleDto> Students { get; set; }

    public List<AdvisorSimpleDTO> Advisors { get; set; }
  }
}
