using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TaskDTOs
{
    public class UpdateTaskDto           
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; }
        public string? AssigneeEmail { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
