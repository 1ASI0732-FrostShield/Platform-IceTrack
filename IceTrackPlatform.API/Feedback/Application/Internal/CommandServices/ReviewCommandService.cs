using IceTrackPlatform.API.Feedback.Domain.Model.Aggregates;
using IceTrackPlatform.API.Feedback.Domain.Model.Commands;
using IceTrackPlatform.API.Feedback.Domain.Repositories;
using IceTrackPlatform.API.Feedback.Domain.Services;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Feedback.Application.Internal.CommandServices;


public class ReviewCommandService(
    IReviewRepository reviewRepository,
    IUnitOfWork unitOfWork) : IReviewCommandService
{
    public async Task<Review?> Handle(CreateReviewCommand command)
    {
        var review = new Review(command.ServiceRequestId, command.OwnerId, command.TechnicianId, command.Comunicacion, command.Eficiencia, command.Profesionalidad, command.Comment);
        await reviewRepository.AddAsync(review);
        await unitOfWork.CompleteAsync();
        return review;
    }
}
