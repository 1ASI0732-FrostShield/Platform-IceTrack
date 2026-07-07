using IceTrackPlatform.API.Feedback.Domain.Model.Aggregates;
using IceTrackPlatform.API.Feedback.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class ReviewTests
{
    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateReview()
    {
        var review = new Review(
            serviceRequestId: 1,
            ownerId: 2,
            technicianId: 3,
            comunicacion: 5,
            eficiencia: 4,
            profesionalidad: 5,
            comment: "Excellent service"
        );

        Assert.AreEqual(1, review.ServiceRequestId);
        Assert.AreEqual(2, review.OwnerId);
        Assert.AreEqual(3, review.TechnicianId);
        Assert.AreEqual("Excellent service", review.Comment);
        Assert.AreEqual(5, review.Rating.Comunicacion);
        Assert.AreEqual(4, review.Rating.Eficiencia);
        Assert.AreEqual(5, review.Rating.Profesionalidad);
    }

    [TestMethod]
    public void EmptyConstructor_WhenCalled_ShouldCreateDefaultReview()
    {
        var review = new Review();

        Assert.AreEqual(string.Empty, review.Comment);
        Assert.AreEqual(0, review.Rating.Comunicacion);
        Assert.AreEqual(0, review.Rating.Eficiencia);
        Assert.AreEqual(0, review.Rating.Profesionalidad);
    }

    [TestMethod]
    public void RatingAverage_WithValidScores_ShouldCalculateCorrectly()
    {
        var rating = new ReviewRating(3, 4, 5);

        var average = rating.Average;

        Assert.AreEqual(4.0, average);
    }
}
