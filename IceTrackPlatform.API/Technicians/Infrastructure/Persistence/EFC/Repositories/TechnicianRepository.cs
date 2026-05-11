using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;
using IceTrackPlatform.API.Technicians.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Technicians.Infrastructure.Persistence.EFC.Repositories;


public class TechnicianRepository(AppDbContext context) : BaseRepository<Technician>(context), ITechnicianRepository
{
    public async Task<IEnumerable<Technician>> FindByProviderIdAsync(int providerId)
    {
        return await Context.Set<Technician>()
            .Where(t => t.ProviderId.Value == providerId && t.DeletedAt == null)
            .ToListAsync();
    }

    public new async Task<IEnumerable<Technician>> ListAsync()
    {
        return await Context.Set<Technician>()
            .Where(t => t.DeletedAt == null)
            .ToListAsync();
    }
}

