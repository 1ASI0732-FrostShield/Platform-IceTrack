using System.ComponentModel.DataAnnotations;

namespace IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

public record UpdateEquipmentResource(
    [Required] string Model,
    [Required] string Type,
    [Required] string Serial,
    [Required] StatusEquipment Status,
    [Required] string Name,
    [Required] int SiteId,
    [Required] bool Online);