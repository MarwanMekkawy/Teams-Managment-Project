using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        public TeamMemberRepository(AppDbContext context) { }       
    }
}
