namespace IceTrackPlatform.API.Feedback.Domain.Model.Commands;


public record CreateReviewCommand(int ServiceRequestId, int OwnerId, int TechnicianId, int Comunicacion, int Eficiencia, int Profesionalidad, string Comment);
