namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

public record MaintenanceHistoryResource(
    int EquipmentId,
    List<ServiceRequestResource> ServiceRequests,
    List<InterventionResource> Interventions);
