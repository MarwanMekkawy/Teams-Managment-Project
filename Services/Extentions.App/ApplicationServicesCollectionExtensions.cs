using Microsoft.Extensions.DependencyInjection;
using Services;
using Services.Abstractions;
using Services.Abstractions.Security;
using Services.MappingProfiles;


namespace Persistance.Extentions
{
    public static class ApplicationServicesCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            // services registering
            services.AddScoped<IOrganizationService,OrganizationService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ITaskService, TaskService>();          
            services.AddScoped<ITeamMemberService, TeamMemberService>();
            services.AddScoped<ITeamService, TeamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();                  // auth service
            

            // Auto mapper service
            services.AddAutoMapper(cfg =>{cfg.AddMaps(typeof(AutoMapperMarker).Assembly);});

            return services;
        }
    }
}
