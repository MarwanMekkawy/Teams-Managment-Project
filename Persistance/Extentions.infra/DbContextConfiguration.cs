using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance;

namespace Infrastructure.Extensions
{
    public static class DbContextConfiguration
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var provider = config["DatabaseProvider"] ?? "SqlServer";
               
                string? connectionString = 
                provider switch
                {
                    "PostgreSql" => config.GetConnectionString("PostgreSqlConnection"),
                    _ => config.GetConnectionString("SqlServerConnection")
                };

                if (provider == "PostgreSql")
                    options.UseNpgsql(connectionString);
                else
                    options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}