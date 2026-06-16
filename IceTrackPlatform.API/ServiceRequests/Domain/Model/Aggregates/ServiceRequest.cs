using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;

public partial class ServiceRequest
{
    public ServiceRequest()
    {
        RequesterId = new RequesterId();
        SiteId = new SiteId();
        EquipmentId = new EquipmentId();
        AssignedTo = new AssignedTo();
        Origin = string.Empty;
        Type = new ServiceRequestType();
        Priority = new ServiceRequestPriority();
        Description = string.Empty;
        Status = new ServiceRequestStatus();
        OwnerId = 0;
    }

    public ServiceRequest(int requesterId, int siteId, int equipmentId, int assignedTo, string origin, EServiceRequestType type, EServiceRequestPriority priority, string description, int ownerId)
    {
        RequesterId = new RequesterId(requesterId);
        SiteId = new SiteId(siteId);
        EquipmentId = new EquipmentId(equipmentId);
        AssignedTo = new AssignedTo(assignedTo);
        Origin = origin;
        Type = new ServiceRequestType(type);
        Priority = new ServiceRequestPriority(priority);
        Description = description;
        Status = new ServiceRequestStatus(EServiceRequestStatus.Pending);
        OwnerId = ownerId;
    }

    public void Accept() => Status = new ServiceRequestStatus(EServiceRequestStatus.Accepted);
    
    public void Reject()
    {
        Status = new ServiceRequestStatus(EServiceRequestStatus.Rejected);
        CanceledAt = DateTime.UtcNow;
    }
    public void Cancel()
    {
        Status = new ServiceRequestStatus(EServiceRequestStatus.Canceled);
        CanceledAt = DateTime.UtcNow;
    }
    public void AssignTechnician(int technicianId)
    {
        TechnicianId = new TechnicianId(technicianId);
        Status = new ServiceRequestStatus(EServiceRequestStatus.InProgress);
    }
    public void Complete()
    {
        Status = new ServiceRequestStatus(EServiceRequestStatus.Completed);
        CompletedAt = DateTime.UtcNow;
    }
    
    public void SoftDelete()
    {
        DeletedAt = DateTimeOffset.UtcNow;
    }
    
    public int Id { get; }
    public int OwnerId { get; private set; }
    public RequesterId RequesterId { get; private set; }
    public SiteId SiteId { get; private set; }
    public EquipmentId EquipmentId { get; private set; }
    public AssignedTo AssignedTo { get; private set; }
    public string Origin { get; private set; }
    public ServiceRequestType Type { get; private set; }
    public ServiceRequestPriority Priority { get; private set; }
    public string Description { get; private set; }
    public ServiceRequestStatus Status { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }
    public TechnicianId? TechnicianId { get; private set; }
}
