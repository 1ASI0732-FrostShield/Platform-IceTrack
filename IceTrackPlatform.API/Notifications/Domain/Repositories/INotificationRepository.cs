using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Notifications.Domain.Repositories;

public interface INotificationRepository : IBaseRepository<Notification>
{
    Task<IEnumerable<Notification>> FindByEquipmentIdAsync(int equipmentId);
    Task<IEnumerable<Notification>> FindActiveByEquipmentIdAsync(int equipmentId);
    Task<IEnumerable<Notification>> FindAllActiveAsync();
    new Task<IEnumerable<Notification>> ListAsync();
}
