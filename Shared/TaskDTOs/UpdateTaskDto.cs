using Domain.Enums;


namespace Shared.TaskDTOs
{
    public class UpdateTaskDto           
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskEntityStatus? Status { get; set; }
        public int? AssigneeId { get; set; }
    }
}
