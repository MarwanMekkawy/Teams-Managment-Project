using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.TeamDTOs
{
    public class TeamDto                
    {
        public string Name { get; set; }
        public string OrganizationName { get; set; }
        public List<string>? MemberNames { get; set; }
        public List<string>? ProjectNames { get; set; }
    }
}
