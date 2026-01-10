using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrganizationDTOs
{
    public class OrganizationDto        
    {
        public string Name { get; set; }
        public List<string>? TeamNames { get; set; }
        public List<string>? UserEmails { get; set; }
    }
}
