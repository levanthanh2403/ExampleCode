using map.backend.shared;
using Serilog.Events;
using Serilog;
using map.backend.shared.Persistence;
using map.backend.shared.Migrations.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructureServices(builder.Configuration);

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                //.ReadFrom.Configuration(configuration)
                .CreateLogger();
Log.Information("Starting up");

var app = builder.Build();

SeedDatabase();
void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var scopedContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            SeedData.Seed(scopedContext);
        }
        catch
        {
            throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // This need to be added	
app.UseAuthorization();

app.MapControllers();

app.Run();
