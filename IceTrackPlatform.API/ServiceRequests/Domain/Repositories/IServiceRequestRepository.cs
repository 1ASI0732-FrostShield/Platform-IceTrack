using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.ServiceRequests.Domain.Repositories;

public interface IServiceRequestRepository : IBaseRepository<ServiceRequest>
{
    Task<IEnumerable<ServiceRequest>> FindByRequesterIdAsync(int requesterId);
    Task<IEnumerable<ServiceRequest>> FindByProviderIdAndStatusAsync(int providerId, EServiceRequestStatus? status);
    Task<IEnumerable<ServiceRequest>> FindByEquipmentIdAsync(int equipmentId);
    new Task<IEnumerable<ServiceRequest>> ListAsync();
}
