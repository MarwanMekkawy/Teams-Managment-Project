using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistance;
using Persistance.Data.Seeding;
using Persistance.Extentions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using TeamsManagmentProject.API.Middleware;

namespace TeamsManagmentProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Di Services via service collection class //
            builder.Services.AddApplicationServices();                              // app layer extentions
            builder.Services.AddInfrastructureServices(builder.Configuration);      // infra layer extentions

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>{options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());});   //json serializing enums

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( 
                // swagger XML comments config //
                options =>{
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });


            // Auth service config //
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
                    };
                });
            builder.Services.AddAuthorization();



            var app = builder.Build();



            // seeding data & migrations for [monster Asp.net] hosting//
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
                var seeder = new DbInitializer(db);
                await seeder.InitializeAsync();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalHandlingMiddleware>();     // global Exception middleware
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
