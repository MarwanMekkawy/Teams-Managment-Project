using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.OrganizationDTOs
{
    public class GetAllOrganizationDto
    {
        public int Id { get; set; }      
        public required string Name { get; set; }
    }
}
