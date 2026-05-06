using System.ComponentModel.DataAnnotations;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;

/// <summary>
///     Represents the resource to create a new equipment.
/// </summary>
public record CreateEquipmentResource(
    [Required] string  Model,
    [Required] string  Type,
    [Required] string  Serial,
    [Required] StatusEquipment  Status,
    [Required] float  SetPoint,
    [Required] string  Name,
    [Required] string  Manufacturer,
    [Required] bool  Online);