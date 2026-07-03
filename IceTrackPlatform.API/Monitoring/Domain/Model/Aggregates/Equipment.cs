using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;

public partial class Equipment : EquipmentAudit
{
    protected Equipment()
    {
        EquipmentId = Guid.NewGuid();
        Model = string.Empty;
        Type = string.Empty;
        Serial = string.Empty;
        Status = StatusEquipment.OFF;
        Name = string.Empty;
        SiteId = 0;
        Online = false;
        OwnerId = 0;
        ReminderIntervalDays = null;
    }
    
    public Equipment(CreateEquipmentCommand command)
    {
        EquipmentId = Guid.NewGuid();
        Model = command.Model;
        Type = command.Type;
        Serial = command.Serial;
        Status = command.Status;
        Name = command.Name;
        SiteId = command.SiteId;
        Online = command.Online;
        OwnerId = command.OwnerId;
        ReminderIntervalDays = command.ReminderIntervalDays;
    }
    
    public void Update(UpdateEquipmentCommand command)
    {
        Model = command.Model;
        Type = command.Type;
        Serial = command.Serial;
        Status = command.Status;
        Name = command.Name;
        SiteId = command.SiteId;
        Online = command.Online;
        ReminderIntervalDays = command.ReminderIntervalDays;
    }
    
    public void SetReminderInterval(int? reminderIntervalDays)
    {
        ReminderIntervalDays = reminderIntervalDays;
    }

    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
    
    public int Id { get; }
    public int OwnerId { get; private set; }
    public Guid EquipmentId { get;}
    public string Model { get; private set; }
    public string Type { get; private set; }
    public string Serial { get; private set; }
    public StatusEquipment Status { get; private set; }
    public string Name { get; private set; }
    public int SiteId { get; private set; }
    public bool Online { get; private set; }
    public int? ReminderIntervalDays { get; private set; }
}