using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class ServiceRequests
{
    private static ServiceRequest CreateValidServiceRequest()
    {
        return new ServiceRequest(
            requesterId: 1,
            siteId: 2,
            equipmentId: 3,
            assignedTo: 4,
            origin: "Web Application",
            type: EServiceRequestType.Corrective,
            priority: EServiceRequestPriority.High,
            description: "The refrigeration equipment is not cooling properly.",
            ownerId: 1
        );
    }

    [TestMethod]
    public void Constructor_WithValidData_ShouldCreatePendingServiceRequest()
    {
        var request = CreateValidServiceRequest();

        Assert.AreEqual(EServiceRequestStatus.Pending, request.Status.Status);
        Assert.AreEqual(EServiceRequestType.Corrective, request.Type.Type);
        Assert.AreEqual(EServiceRequestPriority.High, request.Priority.Priority);
        Assert.AreEqual("The refrigeration equipment is not cooling properly.", request.Description);
    }

    [TestMethod]
    public void Accept_WhenCalled_ShouldChangeStatusToAccepted()
    {
        var request = CreateValidServiceRequest();

        request.Accept();

        Assert.AreEqual(EServiceRequestStatus.Accepted, request.Status.Status);
    }

    [TestMethod]
    public void Reject_WhenCalled_ShouldChangeStatusToRejected()
    {
        var request = CreateValidServiceRequest();

        request.Reject();

        Assert.AreEqual(EServiceRequestStatus.Rejected, request.Status.Status);
    }

    [TestMethod]
    public void Cancel_WhenCalled_ShouldChangeStatusToCanceledAndSetCanceledAt()
    {
        var request = CreateValidServiceRequest();

        request.Cancel();

        Assert.AreEqual(EServiceRequestStatus.Canceled, request.Status.Status);
        Assert.IsNotNull(request.CanceledAt);
    }

    [TestMethod]
    public void AssignTechnician_WithValidTechnicianId_ShouldSetTechnicianAndChangeStatusToInProgress()
    {
        var request = CreateValidServiceRequest();

        request.AssignTechnician(10);

        Assert.IsNotNull(request.TechnicianId);
        Assert.AreEqual(10, request.TechnicianId.Value);
        Assert.AreEqual(EServiceRequestStatus.InProgress, request.Status.Status);
    }

    [TestMethod]
    public void Complete_WhenCalled_ShouldChangeStatusToCompletedAndSetCompletedAt()
    {
        var request = CreateValidServiceRequest();

        request.Complete();

        Assert.AreEqual(EServiceRequestStatus.Completed, request.Status.Status);
        Assert.IsNotNull(request.CompletedAt);
    }
}