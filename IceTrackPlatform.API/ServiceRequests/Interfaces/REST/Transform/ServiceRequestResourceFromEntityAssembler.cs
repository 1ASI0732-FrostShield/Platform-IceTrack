using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;
public static class ServiceRequestResourceFromEntityAssembler
{
    public static ServiceRequestResource ToResourceFromEntity(ServiceRequest entity)
    {
        return new ServiceRequestResource(
            entity.Id,
            entity.OwnerId,
            entity.RequesterId.Value,
            entity.SiteId.Value,
            entity.EquipmentId.Value,
            entity.AssignedTo.Value,
            entity.Origin,
            entity.Type.Type,
            entity.Priority.Priority,
            entity.Description,
            entity.Status.Status,
            entity.CompletedAt,
            entity.CanceledAt,
            entity.TechnicianId?.Value);
    }
}
