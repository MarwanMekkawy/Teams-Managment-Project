using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TeamDTOs
{
    public class CreateTeamDto        
    {
        public string Name { get; set; } 
        public int OrganizationId { get; set; }
    }
}
