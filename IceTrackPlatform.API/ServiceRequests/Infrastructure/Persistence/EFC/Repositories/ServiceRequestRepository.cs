using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.ServiceRequests.Infrastructure.Persistence.EFC.Repositories;

public class ServiceRequestRepository(AppDbContext context) : BaseRepository<ServiceRequest>(context), IServiceRequestRepository
{
    public async Task<IEnumerable<ServiceRequest>> FindByRequesterIdAsync(int requesterId)
    {
        return await Context.Set<ServiceRequest>()
            .Where(sr => sr.RequesterId.Value == requesterId && sr.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<ServiceRequest>> FindByProviderIdAndStatusAsync(int providerId, EServiceRequestStatus? status)
    {
        var query = Context.Set<ServiceRequest>()
            .Where(sr => sr.AssignedTo.Value == providerId && sr.DeletedAt == null);
        if (status.HasValue)
            query = query.Where(sr => sr.Status.Status == status.Value);
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<ServiceRequest>> FindByEquipmentIdAsync(int equipmentId)
    {
        return await Context.Set<ServiceRequest>()
            .Where(sr => sr.EquipmentId.Value == equipmentId && sr.DeletedAt == null)
            .ToListAsync();
    }

    public new async Task<IEnumerable<ServiceRequest>> ListAsync()
    {
        return await Context.Set<ServiceRequest>()
            .Where(sr => sr.DeletedAt == null)
            .ToListAsync();
    }
}
