using Domain.Enums;


namespace Shared.TaskDTOs
{
    public class TaskDto                 
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskEntityStatus Status { get; set; }
        public int ProjectId { get; set; }
        public int? AssigneeId { get; set; }
    }
}
