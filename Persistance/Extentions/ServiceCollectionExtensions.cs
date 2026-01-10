using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Services.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            // Auto mapper service
            services.AddAutoMapper(cfg =>{cfg.AddMaps(typeof(AutoMapperMarker).Assembly);});

            return services;
        }
    }
}
