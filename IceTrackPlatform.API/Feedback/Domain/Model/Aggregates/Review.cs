using IceTrackPlatform.API.Feedback.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.Feedback.Domain.Model.Aggregates;

public partial class Review
{
    public int Id { get; }
    public int ServiceRequestId { get; private set; }
    public int OwnerId { get; private set; }
    public int TechnicianId { get; private set; }
    public ReviewRating Rating { get; private set; }
    public string Comment { get; private set; }

    public Review()
    {
        Comment = string.Empty;
        Rating = new ReviewRating();
    }

    public Review(int serviceRequestId, int ownerId, int technicianId, int comunicacion, int eficiencia, int profesionalidad, string comment)
    {
        ServiceRequestId = serviceRequestId;
        OwnerId = ownerId;
        TechnicianId = technicianId;
        Rating = new ReviewRating(comunicacion, eficiencia, profesionalidad);
        Comment = comment;
    }
}
