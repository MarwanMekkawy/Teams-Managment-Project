using Domain.Contracts;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repositories
{
    public class ProjectRepository : GenericRepository<Project, int> , IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }      
    }
}
