using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Monitoring.Infrastructure.Persistence.EFC.Repositories;

public class EquipmentRepository(AppDbContext context)
    : BaseRepository<Equipment>(context), IEquipmentRepository
{
    public async Task<IEnumerable<Equipment>> FindByTypeAsync(string type)
    {
        return await Context.Set<Equipment>()
            .Where(f => f.Type == type && f.DeletedAt == null)
            .ToListAsync();
    }

    public async Task<bool> ExistsBySerialAsync(string serial, int? excludeId = null)
        => await Context.Set<Equipment>().AnyAsync(e => e.Serial == serial 
                                                        && e.Id != excludeId 
                                                        && e.DeletedAt == null);
}