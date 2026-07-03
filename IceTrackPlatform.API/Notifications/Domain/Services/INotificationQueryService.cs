using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;

namespace IceTrackPlatform.API.Notifications.Domain.Services;

public interface INotificationQueryService
{
    Task<IEnumerable<Notification>> HandleGetAllActive();
    Task<IEnumerable<Notification>> HandleGetByEquipmentId(int equipmentId);
}
