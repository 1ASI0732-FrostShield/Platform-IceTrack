using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;

public static class InterventionResourceFromEntityAssembler
{
    public static InterventionResource ToResourceFromEntity(
        Intervention entity,
        string? technicianName = null)
    {
        return new InterventionResource(
            entity.Id,
            entity.ServiceRequestId.Value,
            entity.TechnicianId.Value,
            entity.Status.Status.ToString(),
            entity.Summary,
            entity.StartTime,
            entity.EndTime,
            entity.PhotoUrls,
            technicianName);
    }
}