using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;
using IceTrackPlatform.API.Notifications.Domain.Repositories;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Notifications.Infrastructure.Persistence.EFC.Repositories;

public class NotificationRepository(AppDbContext context)
    : BaseRepository<Notification>(context), INotificationRepository
{
    public async Task<IEnumerable<Notification>> FindByEquipmentIdAsync(int equipmentId)
    {
        return await Context.Set<Notification>()
            .Where(n => n.EquipmentId == equipmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> FindActiveByEquipmentIdAsync(int equipmentId)
    {
        return await Context.Set<Notification>()
            .Where(n => n.EquipmentId == equipmentId && n.DismissedAt == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> FindAllActiveAsync()
    {
        return await Context.Set<Notification>()
            .Where(n => n.DismissedAt == null)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }

    public new async Task<IEnumerable<Notification>> ListAsync()
    {
        return await Context.Set<Notification>()
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
    }
}
