using IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;

namespace IceTrackPlatform.API.Technicians.Infrastructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;


public static class ModelBuilderExtensions
{
    public static void ApplyTechniciansConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Technician>().HasKey(t => t.Id);
        builder.Entity<Technician>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Technician>().Property(t => t.Name).IsRequired();
        builder.Entity<Technician>().Property(t => t.Specialty).IsRequired();
        builder.Entity<Technician>().Property(t => t.Phone).IsRequired(false);
        builder.Entity<Technician>()
            .OwnsOne(t => t.ProviderId, p =>
            {
                p.Property(id => id.Value).HasColumnName("ProviderId").IsRequired();
            });
        builder.Entity<Technician>().Property(t => t.DeletedAt).IsRequired(false);
    }
}
