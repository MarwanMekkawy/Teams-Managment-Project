using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        public IOrganizationRepository organizations { get; }
        public IProjectRepository projects { get; }
        public ITaskRepository tasks { get; }
        public ITeamMemberRepository teamMembers { get; }
        public ITeamRepository teams { get; }
        public IUserRepository users { get; }
        public Task<int> SaveChangesAsync();
    }
}
