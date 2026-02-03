using Domain.Enums;

namespace Domain.Entities
{
    public class Project:BaseEntity<int>
    {
        public required string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public int TeamId { get; set; }

        // Navigation
        public Team? Team { get; set; }
        public ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();
    }
}
