namespace IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;

public partial class Notification
{
    public Notification()
    {
        Message = string.Empty;
        Type = "maintenance_reminder";
    }

    public Notification(int equipmentId, string message, string type)
    {
        EquipmentId = equipmentId;
        Message = message;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAt = DateTime.UtcNow;
    }

    public void Dismiss()
    {
        DismissedAt = DateTime.UtcNow;
    }

    public int Id { get; }
    public int EquipmentId { get; private set; }
    public string Message { get; private set; }
    public string Type { get; private set; }
    public bool IsRead { get; private set; }
    public DateTime? ReadAt { get; private set; }
    public DateTime? DismissedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
