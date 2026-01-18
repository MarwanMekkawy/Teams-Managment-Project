using Persistance;
using Persistance.Data.Seeding;
using Persistance.Extentions;
using System.Reflection;
using System.Text.Json.Serialization;

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

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // seeding data //
                using (var scope = app.Services.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var seeder = new DbInitializer(db);
                    await seeder.InitializeAsync();
                }

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
