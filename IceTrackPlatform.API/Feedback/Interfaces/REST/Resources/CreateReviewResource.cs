namespace IceTrackPlatform.API.Feedback.Interfaces.REST.Resources;

public record CreateReviewResource(int ServiceRequestId, int OwnerId, int TechnicianId, int Comunicacion, int Eficiencia, int Profesionalidad, string Comment);
