using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class InterventionTests
{
    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateIntervention()
    {
        var photos = new List<string> { "photo1.jpg", "photo2.jpg" };
        var startTime = DateTime.UtcNow.AddHours(-2);
        var endTime = DateTime.UtcNow;

        var intervention = new Intervention(
            serviceRequestId: 1,
            technicianId: 2,
            summary: "Fixed compressor",
            startTime: startTime,
            endTime: endTime,
            photoUrls: photos
        );

        Assert.AreEqual(1, intervention.ServiceRequestId.Value);
        Assert.AreEqual(2, intervention.TechnicianId.Value);
        Assert.AreEqual("Fixed compressor", intervention.Summary);
        Assert.AreEqual(startTime, intervention.StartTime);
        Assert.AreEqual(endTime, intervention.EndTime);
        Assert.AreEqual(EInterventionStatus.Completed, intervention.Status.Status);
        Assert.AreEqual(2, intervention.PhotoUrls.Count);
    }

    [TestMethod]
    public void Constructor_WithoutEndTime_ShouldSetStatusToPending()
    {
        var intervention = new Intervention(
            serviceRequestId: 1,
            technicianId: 2,
            summary: "In progress",
            startTime: DateTime.UtcNow,
            endTime: null,
            photoUrls: new List<string>()
        );

        Assert.AreEqual(EInterventionStatus.Pending, intervention.Status.Status);
        Assert.IsNull(intervention.EndTime);
    }

    [TestMethod]
    public void EmptyConstructor_WhenCalled_ShouldCreateDefaultIntervention()
    {
        var intervention = new Intervention();

        Assert.AreEqual(string.Empty, intervention.Summary);
        Assert.AreEqual(0, intervention.PhotoUrls.Count);
    }

    [TestMethod]
    public void SoftDelete_WhenCalled_ShouldSetDeletedAt()
    {
        var intervention = new Intervention(
            serviceRequestId: 1,
            technicianId: 2,
            summary: "Test",
            startTime: DateTime.UtcNow,
            endTime: null,
            photoUrls: new List<string>()
        );

        intervention.SoftDelete();

        Assert.IsNotNull(intervention.DeletedAt);
    }
}
