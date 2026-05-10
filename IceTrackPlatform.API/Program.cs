using System.Text.Json;
using System.Text.Json.Serialization;
using IceTrackPlatform.API.Assets_Management.Application.Internal.CommandServices;
using IceTrackPlatform.API.Assets_Management.Application.Internal.QueryServices;
using IceTrackPlatform.API.Assets_Management.Domain.Repositories;
using IceTrackPlatform.API.Assets_Management.Domain.Services;
using IceTrackPlatform.API.Assets_Management.Infrastructure.Persistence.EFC.Repositories;
using IceTrackPlatform.API.Dashboard.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using IceTrackPlatform.API.Feedback.Application.Internal.CommandServices;
using IceTrackPlatform.API.Feedback.Application.Internal.QueryServices;
using IceTrackPlatform.API.Feedback.Domain.Repositories;
using IceTrackPlatform.API.Feedback.Domain.Services;
using IceTrackPlatform.API.Feedback.Infrastructure.Persistence.EFC.Repositories;
using IceTrackPlatform.API.IAM.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using IceTrackPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Extensions;
using IceTrackPlatform.API.Monitoring.Application.Internal.CommandServices;
using IceTrackPlatform.API.Monitoring.Application.Internal.QueryServices;
using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.Monitoring.Domain.Services;
using IceTrackPlatform.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;
using IceTrackPlatform.API.ServiceRequests.Application.Internal.CommandServices;
using IceTrackPlatform.API.ServiceRequests.Application.Internal.QueryServices;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;
using IceTrackPlatform.API.ServiceRequests.Infrastructure.Persistence.EFC.Repositories;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Documentation.OpenApi.Configuration.Extensions;
using IceTrackPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration.Extensions;
using IceTrackPlatform.API.Shared.Infrastructure.Mediator.Cortex.Configuration.Extensions;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using IceTrackPlatform.API.Technicians.Application.Internal.CommandServices;
using IceTrackPlatform.API.Technicians.Application.Internal.QueryServices;
using IceTrackPlatform.API.Technicians.Domain.Repositories;
using IceTrackPlatform.API.Technicians.Domain.Services;
using IceTrackPlatform.API.Technicians.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllPolicy",
        policy => policy.AllowAnyOrigin()
            .AllowAnyMethod().AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()))
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

// Add Database Services (Configuración de la base de datos)
builder.AddDatabaseServices();

// Open API Configuration (Configuración de Swagger)
builder.AddOpenApiDocumentationServices();
builder.AddSharedContextServices();
builder.AddIamContextServices();
builder.AddDashboardContextServices();

// Assets Management Bounded Context Injections
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<ISiteCommandService, SiteCommandService>();
builder.Services.AddScoped<ISiteQueryServices, SiteQueryService>();

// Monitoring Bounded Context Injections
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IEquipmentCommandService, EquipmentCommandService>();
builder.Services.AddScoped<IEquipmentQueryServices, EquipmentQueryService>();

// Service Requests Bounded Context Injections
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
builder.Services.AddScoped<IServiceRequestCommandService, ServiceRequestCommandService>();
builder.Services.AddScoped<IServiceRequestQueryService, ServiceRequestQueryService>();
    

builder.Services.AddScoped<IInterventionRepository, InterventionRepository>();
builder.Services.AddScoped<IInterventionCommandService, InterventionCommandService>();
builder.Services.AddScoped<IInterventionQueryService, InterventionQueryService>();

// Technicians Bounded Context Injections
builder.Services.AddScoped<ITechnicianRepository, TechnicianRepository>();
builder.Services.AddScoped<ITechnicianCommandService, TechnicianCommandService>();
builder.Services.AddScoped<ITechnicianQueryService, TechnicianQueryService>();


builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewCommandService, ReviewCommandService>();
builder.Services.AddScoped<IReviewQueryService, ReviewQueryService>();

// Mediator Configuration
builder.AddCortexConfigurationServices();

//Render configs
 var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

 builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

 builder.WebHost.ConfigureKestrel(opt =>
 {
     opt.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// Verify if the database exists and create it if it doesn't
app.UseDatabaseCreationAssurance();

// Apply CORS Policy
app.UseCors("AllowAllPolicy");

// Configure the HTTP request pipeline.
app.UseOpenApiDocumentation();
app.UseHttpsRedirection();

app.UseAuthorization();
app.UseRequestAuthorization();
app.MapControllers();
app.Run();
