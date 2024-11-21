namespace VicemAPI.DTOs.Student
{
  public class CreateStudentDto
  {
    public string Code { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Class { get; set; }

    public string Course { get; set; }

    public int DepartmentID { get; set; }

    public int? AdvisorID { get; set; }

    public string? Description { get; set; }
  }
}