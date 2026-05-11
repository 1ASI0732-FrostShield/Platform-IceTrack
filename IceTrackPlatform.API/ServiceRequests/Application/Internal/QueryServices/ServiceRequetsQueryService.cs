using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Queries;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;

namespace IceTrackPlatform.API.ServiceRequests.Application.Internal.QueryServices;

public class ServiceRequestQueryService(IServiceRequestRepository serviceRequestRepository) : IServiceRequestQueryService
{
    public async Task<ServiceRequest?> Handle(GetServiceRequestByIdQuery query)
    {
        var sr = await serviceRequestRepository.FindByIdAsync(query.ServiceRequestId);
        if (sr?.DeletedAt != null) return null;
        return sr;
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetAllServiceRequestsQuery query)
    {
        var list = await serviceRequestRepository.ListAsync();
        return list.Where(sr => sr.DeletedAt == null);
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByRequesterIdQuery query)
    {
        var list = await serviceRequestRepository.FindByRequesterIdAsync(query.RequesterId);
        return list.Where(sr => sr.DeletedAt == null);
    }

    public async Task<IEnumerable<ServiceRequest>> Handle(GetServiceRequestsByProviderIdQuery query)
    {
        var list = await serviceRequestRepository.FindByProviderIdAndStatusAsync(query.ProviderId, query.Status);
        return list.Where(sr => sr.DeletedAt == null);
    }
}