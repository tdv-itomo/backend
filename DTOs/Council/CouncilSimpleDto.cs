namespace VicemAPI.DTOs.Council
{
    public class CouncilSimpleDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime ReviewTime { get; set; }

        public string Location { get; set; }

        public int DepartmentID { get; set; }

        public DepartmentDTO Department { get; set; }
    }
}