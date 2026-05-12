using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IcetrackTest.Monitoring;

    [TestClass]
    public class WebApplicationBuilderExtensionsTests
    {
        [TestMethod]
        public void AddDatabaseServices_ShouldRegisterDbContext_InDevelopment()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["ConnectionStrings:DefaultConnection"] = 
                "Server=localhost;Database=IceTrack;User=root;Password=root;";
            builder.Environment.EnvironmentName = "Development";

            // Act
            builder.AddDatabaseServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<AppDbContext>();
            Assert.IsNotNull(dbContext);
        }

        [TestMethod]
        public void AddDatabaseServices_ShouldRegisterDbContext_InProduction()
        {
            // Arrange
            var builder = WebApplication.CreateBuilder();
            builder.Configuration["ConnectionStrings:DefaultConnection"] = 
                "Server=localhost;Database=IceTrack;User=root;Password=root;";
            builder.Environment.EnvironmentName = "Production";

            // Act
            builder.AddDatabaseServices();

            // Assert
            var serviceProvider = builder.Services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<AppDbContext>();
            Assert.IsNotNull(dbContext);
        }
    }

