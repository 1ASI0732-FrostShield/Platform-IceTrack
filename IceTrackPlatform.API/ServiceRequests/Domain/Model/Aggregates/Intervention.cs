using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;

public partial class Intervention
{
    public Intervention()
    {
        ServiceRequestId = new ServiceRequestId();
        TechnicianId = new TechnicianId();
        Status = new InterventionStatus();
        Summary = string.Empty;
        PhotoUrls = new List<string>();
    }

    public Intervention(int serviceRequestId, int technicianId, string summary, DateTime startTime, DateTime? endTime, List<string> photoUrls)
    {
        ServiceRequestId = new ServiceRequestId(serviceRequestId);
        TechnicianId = new TechnicianId(technicianId);
        Summary = summary;
        StartTime = startTime;
        EndTime = endTime;
        Status = endTime.HasValue ? new InterventionStatus(EInterventionStatus.Completed) : new InterventionStatus(EInterventionStatus.Pending);
        PhotoUrls = photoUrls;
    }
    
    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
    
    public int Id { get; }
    public ServiceRequestId ServiceRequestId { get; private set; }
    public TechnicianId TechnicianId { get; private set; }
    public InterventionStatus Status { get; private set; }
    public string Summary { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public List<string> PhotoUrls { get; private set; }
}
