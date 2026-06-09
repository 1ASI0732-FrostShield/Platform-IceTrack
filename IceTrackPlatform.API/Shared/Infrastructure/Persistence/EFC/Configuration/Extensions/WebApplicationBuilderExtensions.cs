using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void AddDatabaseServices(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        if (connectionString == null) throw new InvalidOperationException("Connection string not found.");

        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            if (builder.Environment.IsDevelopment())
                options.UseNpgsql(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            else if (builder.Environment.IsProduction())
                options.UseNpgsql(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Error);
        });
    }
}