using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Monitoring.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyMonitoringConfiguration(this ModelBuilder builder)
    {
        // Assets Management Context
        builder.Entity<Equipment>().HasKey(e => e.Id);
        builder.Entity<Equipment>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Equipment>().Property(e => e.EquipmentId).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Equipment>().Property(e => e.Model).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Type).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Serial).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Status).IsRequired();
        builder.Entity<Equipment>().Property(e => e.SetPoint).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Name).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Manufacturer).IsRequired();
        builder.Entity<Equipment>().Property(e => e.Online).IsRequired();
        //Alerts
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.TenantId).IsRequired();
        builder.Entity<Alert>().Property(a => a.EquipmentId).IsRequired();
        builder.Entity<Alert>().Property(a => a.SiteId).IsRequired();
        builder.Entity<Alert>().Property(a => a.Type).IsRequired();
        builder.Entity<Alert>().Property(a => a.Status).HasConversion<string>().IsRequired();
        builder.Entity<Alert>().Property(a => a.Severity).HasConversion<string>().IsRequired();
    }
}