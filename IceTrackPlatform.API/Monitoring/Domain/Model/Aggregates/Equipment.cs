using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;

public partial class Equipment : EquipmentAudit
{
    public int Id { get; }
    public Guid EquipmentId { get;}
    public string Model { get; private set; }
    public string Type { get; private set; }
    public string Serial { get; private set; }
    public StatusEquipment Status { get; private set; }
    public string Name { get; private set; }
    public int SiteId { get; private set; }
    public bool Online { get; private set; }

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
    }
}