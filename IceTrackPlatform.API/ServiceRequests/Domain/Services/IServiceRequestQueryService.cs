using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Queries;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Services;

public interface IServiceRequestQueryService
{
    Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query);
    Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByRequesterIdQuery query);
    Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByProviderIdQuery query);
    
    Task<IEnumerable<ServiceRequest>> Handle(GetAllServiceRequestsQuery query);
}