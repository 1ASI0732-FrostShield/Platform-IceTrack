namespace IceTrackPlatform.API.Feedback.Interfaces.REST.Resources;
using IceTrackPlatform.API.Feedback.Domain.Model.Aggregates;

public record ReviewResource(
    int Id,
    int ServiceRequestId,
    int OwnerId,
    int TechnicianId,
    int Comunicacion,
    int Eficiencia,
    int Profesionalidad,
    double Average,
    string Comment);
