using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Team : BaseEntity<int>
    {
        public string Name { get; set; }
        public int OrganizationId { get;  set; }

        // Navigation
        public Organization Organization { get; set; }
        public ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();
    }
}
