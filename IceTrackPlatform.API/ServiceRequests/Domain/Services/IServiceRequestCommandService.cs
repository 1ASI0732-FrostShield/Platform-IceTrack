using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Commands;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Services;

public interface IServiceRequestCommandService
{
    Task<ServiceRequest?> Handle(CreateServiceRequestCommand command);
    Task<ServiceRequest?> Handle(AcceptServiceRequestCommand command);
    Task<ServiceRequest?> Handle(RejectServiceRequestCommand command);
    Task<ServiceRequest?> Handle(CancelServiceRequestCommand command);
    Task<ServiceRequest?> Handle(AssignTechnicianToServiceRequestCommand command);
    Task<ServiceRequest?> Handle(CompleteServiceRequestCommand command);
    Task<bool> Handle(DeleteServiceRequestCommand command);
}