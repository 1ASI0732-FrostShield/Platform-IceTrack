using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform CreateEquipmentResource to CreateEquipmentCommand.
/// </summary>
public static class CreateEquipmentCommandFromResourceAssembler
{
    public static CreateEquipmentCommand ToCommandFromResource(CreateEquipmentResource resource) =>
        new CreateEquipmentCommand(resource.Model, resource.Type, resource.Serial,
            resource.Status, resource.Name, resource.SiteId, resource.Online);
}