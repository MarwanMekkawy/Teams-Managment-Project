using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ProjectDTOs
{
    public class ProjectDto              // GET /projects
    {
        public string Name { get; set; }
        public ProjectStatus Status { get; set; }
        public string TeamName { get; set; }
        public List<string>? TaskTitles { get; set; }
    }
}
