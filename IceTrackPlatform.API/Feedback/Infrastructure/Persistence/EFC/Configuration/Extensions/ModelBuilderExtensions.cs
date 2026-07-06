using IceTrackPlatform.API.Feedback.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Feedback.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyFeedbackConfiguration(this ModelBuilder builder)
    {
        // Review Aggregate
        builder.Entity<Review>().HasKey(r => r.Id);
        builder.Entity<Review>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Review>().Property(r => r.ServiceRequestId).IsRequired();
        builder.Entity<Review>().Property(r => r.OwnerId).IsRequired();
        builder.Entity<Review>().Property(r => r.TechnicianId).IsRequired();
        builder.Entity<Review>().Property(r => r.Comment).IsRequired(false);

        builder.Entity<Review>()
            .OwnsOne(r => r.Rating, rr =>
            {
                rr.Property(p => p.Comunicacion).HasColumnName("comunicacion");
                rr.Property(p => p.Eficiencia).HasColumnName("eficiencia");
                rr.Property(p => p.Profesionalidad).HasColumnName("profesionalidad");
            });
    }
}
