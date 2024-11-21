using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VicemAPI.DTOs.Council
{
    public class CreateCouncilDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime ReviewTime { get; set; }

        public string Location { get; set; }

        public int DepartmentID { get; set; }

        public List<int> StudentIds { get; set; }

        public List<int> AdvisorIds { get; set; }

    }
}