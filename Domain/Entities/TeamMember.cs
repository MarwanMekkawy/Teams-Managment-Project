using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TeamMember
    {
        public int TeamId { get; set; }
        public int UserId { get; set; }

        // Navigation
        public Team Team { get; set; }
        public User User { get; set; }
    }
}
