using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Services.MappingProfiles;


namespace Persistance.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
        {
            //DbContext Connection String 
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            // Unit of work service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // repositories
            //services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            //services.AddScoped<IProjectRepository, ProjectRepository>();
            //services.AddScoped<ITaskRepository, TaskRepository>();
            //services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            //services.AddScoped<ITeamRepository, TeamRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();

            // Auto mapper service
            services.AddAutoMapper(cfg =>{cfg.AddMaps(typeof(AutoMapperMarker).Assembly);});

            return services;
        }
    }
}
