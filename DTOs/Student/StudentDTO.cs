using VicemAPI.DTOs.Advisor;
using VicemAPI.DTOs.Topic;
using VicemAPI.DTOs.Council;
using VicemAPI.Models.Entities;

namespace VicemAPI.DTOs.Student
{
  public class StudentDTO
  {
    public int ID { get; set; }

    public string Code { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Class { get; set; }

    public string Course { get; set; }

    public int DepartmentID { get; set; }

    public DepartmentDTO? Department { get; set; }

    public int? AdvisorID { get; set; }

    public AdvisorSimpleDTO? Advisor { get; set; }

    public int? CompanyID { get; set; }

    public Company? Company { get; set; }

    public int? CouncilID { get; set; }

    public CouncilSimpleDto? Council { get; set; }

    public int? TopicID { get; set; }

    public TopicDto? Topic { get; set; }

    public string Status { get; set; } = "Mới";

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public float? FirstScore { get; set; }

    public float? SecondScore { get; set; }

    public float? ThirdScore { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
  }
}