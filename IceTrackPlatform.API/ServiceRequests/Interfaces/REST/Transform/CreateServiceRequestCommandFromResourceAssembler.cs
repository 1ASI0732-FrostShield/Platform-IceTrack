using IceTrackPlatform.API.ServiceRequests.Domain.Model.Commands;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;

public static class CreateServiceRequestCommandFromResourceAssembler
{
    public static CreateServiceRequestCommand ToCommandFromResource(CreateServiceRequestResource resource, int ownerId)
    {
        return new CreateServiceRequestCommand(
            resource.RequesterId,
            resource.SiteId,
            resource.EquipmentId,
            resource.AssignedTo,
            resource.Origin,
            (EServiceRequestType)Enum.Parse(typeof(EServiceRequestType), resource.Type, true),
            (EServiceRequestPriority)Enum.Parse(typeof(EServiceRequestPriority), resource.Priority, true),
            resource.Description,
            ownerId);
    }
}