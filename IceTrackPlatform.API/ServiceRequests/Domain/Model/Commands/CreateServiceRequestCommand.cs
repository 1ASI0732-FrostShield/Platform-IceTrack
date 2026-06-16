using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Model.Commands;

public record CreateServiceRequestCommand(
    int RequesterId,
    int SiteId,
    int EquipmentId,
    int AssignedTo,
    string Origin,
    EServiceRequestType Type,
    EServiceRequestPriority Priority,
    string Description,
    int OwnerId
    );