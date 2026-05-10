using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Queries;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;

namespace IceTrackPlatform.API.ServiceRequests.Application.Internal.QueryServices;

public class ServiceRequestQueryService(IServiceRequestRepository serviceRequestRepository) : IServiceRequestQueryService
{
    public async Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query)
    {
        return await serviceRequestRepository.FindByIdAsync(query.ServiceRequestId);
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByRequesterIdQuery query)
    {
        return await serviceRequestRepository.FindByRequesterIdAsync(query.RequesterId);
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByProviderIdQuery query)
    {
        return await serviceRequestRepository.FindByProviderIdAndStatusAsync(query.ProviderId, query.Status);
    }
    
    public async Task<IEnumerable<ServiceRequest>> Handle(GetAllServiceRequestsQuery query)
    {
        return await serviceRequestRepository.ListAsync();
    }
}