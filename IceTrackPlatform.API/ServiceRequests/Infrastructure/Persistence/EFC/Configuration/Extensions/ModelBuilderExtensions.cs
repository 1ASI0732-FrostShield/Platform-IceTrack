using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.ServiceRequests.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyServiceRequestsConfiguration(this ModelBuilder builder)
    {
        // ServiceRequest Aggregate
        builder.Entity<ServiceRequest>().HasKey(sr => sr.Id);
        builder.Entity<ServiceRequest>().Property(sr => sr.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ServiceRequest>().Property(sr => sr.Origin).IsRequired();
        builder.Entity<ServiceRequest>().Property(sr => sr.Description).IsRequired();
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.RequesterId, r =>
            {
                r.Property(id => id.Value).HasColumnName("RequesterId").IsRequired(); 
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.SiteId, s =>
            {
                s.Property(id => id.Value).HasColumnName("SiteId").IsRequired(); 
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.EquipmentId, e =>
            {
                e.Property(id => id.Value).HasColumnName("EquipmentId").IsRequired(); 
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.AssignedTo, a =>
            {
                a.Property(id => id.Value).HasColumnName("AssignedTo").IsRequired(); 
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.TechnicianId, t =>
            {
                t.Property(id => id.Value).HasColumnName("TechnicianId"); // Removed .IsRequired(false)
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.Type, t =>
            {
                t.Property(type => type.Type).HasColumnName("Type").HasConversion<string>().IsRequired();
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.Priority, p =>
            {
                p.Property(priority => priority.Priority).HasColumnName("Priority").HasConversion<string>().IsRequired();
            });
        builder.Entity<ServiceRequest>()
            .OwnsOne(sr => sr.Status, s =>
            {
                s.Property(status => status.Status).HasColumnName("Status").HasConversion<string>().IsRequired();
            });
        builder.Entity<ServiceRequest>().Property(sr => sr.DeletedAt).IsRequired(false);

        // Intervention Aggregate
        builder.Entity<Intervention>().HasKey(i => i.Id);
        builder.Entity<Intervention>().Property(i => i.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Intervention>().Property(i => i.Summary).IsRequired();
        builder.Entity<Intervention>().Property(i => i.StartTime).IsRequired();
        builder.Entity<Intervention>().Property(i => i.PhotoUrls).HasConversion(
            v => string.Join(",", v),
            v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
        );
        builder.Entity<Intervention>()
            .OwnsOne(i => i.ServiceRequestId, sr =>
            {
                sr.Property(id => id.Value).HasColumnName("ServiceRequestId").IsRequired(); 
            });
        builder.Entity<Intervention>()
            .OwnsOne(i => i.TechnicianId, t =>
            {
                t.Property(id => id.Value).HasColumnName("TechnicianId").IsRequired(); 
            });
        builder.Entity<Intervention>()
            .OwnsOne(i => i.Status, s =>
            {
                s.Property(status => status.Status).HasColumnName("Status").HasConversion<string>().IsRequired();
            });
        builder.Entity<Intervention>().Property(i => i.DeletedAt).IsRequired(false);
    }
}
