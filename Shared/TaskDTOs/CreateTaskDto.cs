using Domain.Enums;

namespace Shared.TaskDTOs
{
    public class CreateTaskDto          
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskEntityStatus Status { get; set; } = TaskEntityStatus.ToDo;
        public int ProjectId { get; set; }
        public int? AssigneeId { get; set; }
    }
}
