using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Project:BaseEntity<int>
    {
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public int TeamId { get; set; }

        // Navigation
        public Team Team { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
