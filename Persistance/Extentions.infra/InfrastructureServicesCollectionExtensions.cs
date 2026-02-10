using Domain.Contracts;
using Domain.Contracts.IRefreshTokens;
using Domain.Contracts.Security;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Repositories.Hash;
using Persistance.Repositories.RefreshTokens;
using Persistance.Security;
using Services.Abstractions.Security;
using Services.RefreshToken;



namespace Persistance.Extentions
{
    public static class InfrastructureServicesCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config) 
        {
            //DbContext Connection String 
            services.AddAppDbContext(config);
          
            // repositories registering
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();        // refresh token repo


            // Unit of work service
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Hasher service
            services.AddSingleton<IPasswordHasher, AppPasswordHasher>();

            // Jwt Token service
            services.AddSingleton<IJwtTokenService, JwtTokenService>();

            // refresh token service
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();  

            return services;
        }
    }
}
