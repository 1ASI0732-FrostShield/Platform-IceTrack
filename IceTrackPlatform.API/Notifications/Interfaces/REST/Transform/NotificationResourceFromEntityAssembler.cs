using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;
using IceTrackPlatform.API.Notifications.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Notifications.Interfaces.REST.Transform;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification entity) =>
        new NotificationResource(
            entity.Id,
            entity.EquipmentId,
            entity.Message,
            entity.Type,
            entity.IsRead,
            entity.ReadAt,
            entity.DismissedAt,
            entity.CreatedAt);
}
