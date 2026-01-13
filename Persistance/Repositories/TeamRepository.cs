using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class TeamRepository : GenericRepository<Team , int> , ITeamRepository
    {
        public TeamRepository(AppDbContext context) : base(context) { }
    }
}
