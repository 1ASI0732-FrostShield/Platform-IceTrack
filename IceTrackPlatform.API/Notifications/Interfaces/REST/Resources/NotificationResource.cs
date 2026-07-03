namespace IceTrackPlatform.API.Notifications.Interfaces.REST.Resources;

public record NotificationResource(
    int Id,
    int EquipmentId,
    string Message,
    string Type,
    bool IsRead,
    DateTime? ReadAt,
    DateTime? DismissedAt,
    DateTime CreatedAt);
