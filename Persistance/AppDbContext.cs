using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }

        public DbSet<Organization> Organizations { get; set; }   
        public DbSet<Project> Projects { get; set; }    
        public DbSet<Domain.Entities.TaskEntity> Tasks { get; set; }
        public DbSet <Team> Teams { get; set; } 
        public DbSet<TeamMember> TeamMembers { get; set; }  
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
