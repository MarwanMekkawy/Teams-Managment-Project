using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TaskEntity : BaseEntity<int>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
        public int? AssigneeId { get; set; }

        // Navigation
        public Project Project { get; private set; }
        public User Assignee { get; private set; }
    }
}
