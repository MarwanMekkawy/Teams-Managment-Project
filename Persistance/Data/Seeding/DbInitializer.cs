using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistance.Data.Seeding
{
    public class DbInitializer
    {
        private readonly AppDbContext _context;

        public DbInitializer(AppDbContext context)
        {
            _context = context; 
        }

        public async System.Threading.Tasks.Task InitializeAsync()
        {
            if (!_context.Organizations.Any())
            {
                var OrgsData = File.ReadAllText(@"../Persistance/Data/Seeding/organizations.json");
                var Orgs=JsonSerializer.Deserialize<List<Organization>>(OrgsData);
                if (Orgs is not null && Orgs.Any())
                {
                    await _context.Organizations.AddRangeAsync(Orgs);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Organizations Seeded [1/6]");
                }
            }
            if (!_context.Users.Any())
            {
                var usersData = File.ReadAllText(@"../Persistance/Data/Seeding/users.json");
                var users = JsonSerializer.Deserialize<List<User>>(usersData);

                if (users is not null && users.Any())
                {
                    await _context.Users.AddRangeAsync(users);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Users Seeded [2/6]");
                }
            }
            if (!_context.Teams.Any())
            {
                var teamsData = File.ReadAllText(@"../Persistance/Data/Seeding/teams.json");
                var teams = JsonSerializer.Deserialize<List<Team>>(teamsData);

                if (teams is not null && teams.Any())
                {
                    await _context.Teams.AddRangeAsync(teams);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Teams Seeded [3/6]");
                }
            }
            if (!_context.TeamMembers.Any())
            {
                var teamMembersData = File.ReadAllText(@"../Persistance/Data/Seeding/teamMembers.json");
                var teamMembers = JsonSerializer.Deserialize<List<TeamMember>>(teamMembersData);

                if (teamMembers is not null && teamMembers.Any())
                {
                    await _context.TeamMembers.AddRangeAsync(teamMembers);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("TeamMembers Seeded [4/6]");
                }
            }
            if (!_context.Projects.Any())
            {
                var ProjectsData = File.ReadAllText(@"../Persistance/Data/Seeding/Projects.json");
                var Projects = JsonSerializer.Deserialize<List<Project>>(ProjectsData);
                if (Projects is not null && Projects.Any())
                {
                    await _context.Projects.AddRangeAsync(Projects);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Projects Seeded [5/6]");

                }
            }
            if (!_context.Tasks.Any())
            {
                var tasksData = File.ReadAllText(@"../Persistance/Data/Seeding/tasks.json");
                var tasks = JsonSerializer.Deserialize<List<Domain.Entities.TaskEntity>>(tasksData);

                if (tasks is not null && tasks.Any())
                {
                    await _context.Tasks.AddRangeAsync(tasks);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Tasks Seeded [6/6]");

                }
            }
        }
    }
}
