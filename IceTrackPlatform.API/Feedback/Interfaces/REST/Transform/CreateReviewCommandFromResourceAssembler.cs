using IceTrackPlatform.API.Feedback.Domain.Model.Commands;
using IceTrackPlatform.API.Feedback.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Feedback.Interfaces.REST.Transform;

public static class CreateReviewCommandFromResourceAssembler
{
    public static CreateReviewCommand ToCommandFromResource(CreateReviewResource resource)
    {
        return new CreateReviewCommand(
            resource.ServiceRequestId,
            resource.OwnerId,
            resource.TechnicianId,
            resource.Comunicacion,
            resource.Eficiencia,
            resource.Profesionalidad,
            resource.Comment);
    }
}