namespace VicemAPI.DTOs.Topic
{
  public class TopicDto
  {
    public int ID { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public string Status { get; set; } = "Chờ duyệt đề tài";
  }
}