using Domain.Enums;

namespace Domain.Entities
{
    public class TaskEntity : BaseEntity<int>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskEntityStatus Status { get; set; }
        public int ProjectId { get; set; }
        public int? AssigneeId { get; set; }

        // Navigation
        public Project Project { get; set; }
        public User Assignee { get; set; }
    }
}
