using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST.Transform;

public static class UpdateEquipmentCommandFromResourceAssembler
{
    public static UpdateEquipmentCommand ToCommandFromResource(int id, UpdateEquipmentResource resource) =>
        new UpdateEquipmentCommand(id, resource.Model, resource.Type, resource.Serial,
            resource.Status, resource.Name, resource.SiteId, resource.Online);
}