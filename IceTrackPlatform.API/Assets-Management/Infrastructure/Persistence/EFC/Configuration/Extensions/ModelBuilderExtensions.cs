using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Assets_Management.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyAssetsManagementConfiguration(this ModelBuilder builder)
    {
        // Assets Management Context
        builder.Entity<Site>().HasKey(s => s.Id);
        builder.Entity<Site>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Site>().Property(s => s.Name).IsRequired();
        builder.Entity<Site>().Property(s => s.Address).IsRequired();
        builder.Entity<Site>().Property(s => s.ContactName).IsRequired();
        builder.Entity<Site>().Property(s => s.Phone).IsRequired();
        builder.Entity<Site>().Property(s => s.CantEquipment).HasDefaultValue(0);
        builder.Entity<Site>().Property(s => s.DeletedAt).IsRequired(false);
    }
}