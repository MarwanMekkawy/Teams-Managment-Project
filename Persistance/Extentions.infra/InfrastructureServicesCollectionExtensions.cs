using Domain.Contracts;
using Domain.Contracts.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Repositories.Hash;



namespace Persistance.Extentions
{
    public static class InfrastructureServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config) 
        {
            //DbContext Connection String 
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
          
            // repositories registering
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Unit of work service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Hasher service
            services.AddSingleton<IPasswordHasher, AppPasswordHasher>();

            return services;
        }
    }
}
