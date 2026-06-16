using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

public record ServiceRequestResource(
    int Id,
    int OwnerId,
    int RequesterId,
    int SiteId,
    int EquipmentId,
    int AssignedTo,
    string Origin,
    EServiceRequestType Type,
    EServiceRequestPriority Priority,
    string Description,
    EServiceRequestStatus Status,
    DateTime? CompletedAt,
    DateTime? CanceledAt,
    int? TechnicianId);