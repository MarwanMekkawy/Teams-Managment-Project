using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TaskDTOs
{
    public class ChangeTaskStatusDto
    {
        public TaskEntityStatus? Status { get; set; }
    }
}
