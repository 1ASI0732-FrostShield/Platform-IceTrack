using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Notifications.Infrastructure.Persistence.EFC.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyNotificationsConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Notification>().HasKey(n => n.Id);
        builder.Entity<Notification>().Property(n => n.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Notification>().Property(n => n.EquipmentId).IsRequired();
        builder.Entity<Notification>().Property(n => n.Message).IsRequired();
        builder.Entity<Notification>().Property(n => n.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Notification>().Property(n => n.IsRead).IsRequired().HasDefaultValue(false);
        builder.Entity<Notification>().Property(n => n.ReadAt).IsRequired(false);
        builder.Entity<Notification>().Property(n => n.DismissedAt).IsRequired(false);
        builder.Entity<Notification>().Property(n => n.CreatedAt).IsRequired();
    }
}
