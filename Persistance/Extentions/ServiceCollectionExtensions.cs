using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Services;
using Services.Abstractions;
using Services.MappingProfiles;


namespace Persistance.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
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

            // services registering
            services.AddScoped<IOrganizationService,OrganizationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();          
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();

            // Auto mapper service
            services.AddAutoMapper(cfg =>{cfg.AddMaps(typeof(AutoMapperMarker).Assembly);});

            return services;
        }
    }
}
