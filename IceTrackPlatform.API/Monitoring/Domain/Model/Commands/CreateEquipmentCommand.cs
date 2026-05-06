using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.Commands;

/// <summary>
///     Command to create a Equipment aggregate
/// </summary>
/// <param name="Model">The Model obtained from equipment</param>
/// <param name="Type">The Type of the equipment</param>
/// <param name="Serial">The Serial of the equipment</param>
/// <param name="Status">The Status of the equipment</param>
/// <param name="SetPoint">The Set Point of the equipment</param>
/// <param name="Manufacturer">The Manufacturer of the equipment</param>
/// <param name="Online">The Online of the equipment</param>
public record CreateEquipmentCommand(string Model, string Type, string Serial, 
    StatusEquipment Status, float SetPoint, string Name, string Manufacturer, bool Online);