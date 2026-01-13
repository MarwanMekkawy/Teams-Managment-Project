using Domain.Enums;


namespace Shared.TaskDTOs
{
    public class TaskDto                 
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskEntityStatus Status { get; set; }
        public string ProjectName { get; set; }
        public string? AssigneeEmail { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
