using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;

/// <summary>
///     Represents the resource for a new equipment.
/// </summary>
public record EquipmentResource(int Id, Guid EquipmentId, string Model, string Type, string Serial, StatusEquipment Status,
    float SetPoint, string Name, string Manufacturer, bool Online,
    DateTimeOffset? Created, DateTimeOffset? Updated);