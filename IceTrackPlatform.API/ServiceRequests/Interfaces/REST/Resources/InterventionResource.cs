namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;

public record InterventionResource(
    int Id,
    int ServiceRequestId,
    int TechnicianId,
    string Status,
    string Summary,
    DateTime StartTime,
    DateTime? EndTime,
    List<string> PhotoUrls,
    string? TechnicianName = null);