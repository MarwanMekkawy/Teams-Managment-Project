using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Concurrent;

namespace Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly AppDbContext _context;
        public IOrganizationRepository organizations { get; }
        public IProjectRepository projects { get; }
        public ITaskRepository tasks { get; }
        public ITeamMemberRepository teamMembers { get; }
        public ITeamRepository teams { get; }
        public IUserRepository users { get; }

        public UnitOfWork(AppDbContext context, IOrganizationRepository organizationRepo, IProjectRepository projectRepo, ITaskRepository taskRepo,
                          ITeamMemberRepository teamMemberRepo, ITeamRepository teamRepo, IUserRepository userRepo)
        {
            _context = context;
            organizations = organizationRepo;
            projects = projectRepo;
            tasks = taskRepo;
            teamMembers = teamMemberRepo;
            teams = teamRepo;
            users = userRepo;
        }
        public async Task<int> SaveChangesAsync()
        {
            // update UpdatedAt time 
            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity<int>>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = null;
                }
            }
            return await _context.SaveChangesAsync();
        }
    }
}
