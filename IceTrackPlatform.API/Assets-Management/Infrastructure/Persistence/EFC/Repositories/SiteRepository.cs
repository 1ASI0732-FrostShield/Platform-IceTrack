using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Domain.Repositories;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using IceTrackPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IceTrackPlatform.API.Assets_Management.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
///     Site Repository Implementation
/// </summary>
/// <remarks>
///     This class implements the repository pattern for managing Site entities using Entity Framework Core.
///     It provides methods to perform CRUD operations and custom queries specific to Site.
/// </remarks>
/// <param name="context"></param>
public class SiteRepository(AppDbContext context)
    : BaseRepository<Site>(context), ISiteRepository
{
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        => await Context.Set<Site>().AnyAsync(s => s.Name == name && s.Id != excludeId);

    public async Task<bool> ExistsByAddressAsync(string address, int? excludeId = null)
        => await Context.Set<Site>().AnyAsync(s => s.Address == address && s.Id != excludeId);

    public async Task<bool> ExistsByPhoneAsync(string phone, int? excludeId = null)
        => await Context.Set<Site>().AnyAsync(s => s.Phone == phone && s.Id != excludeId);
    
    public async Task<Site?> FindByNameAsync(string name)
        => await Context.Set<Site>().FirstOrDefaultAsync(s => s.Name == name);
}