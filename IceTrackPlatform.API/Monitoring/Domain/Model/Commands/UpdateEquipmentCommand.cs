using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.Commands;

public record UpdateEquipmentCommand(
    int EquipmentId,
    string Model, 
    string Type, 
    string Serial,
    StatusEquipment Status, 
    string Name, 
    int SiteId,
    bool Online
    );