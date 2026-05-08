using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Assets_Management.Domain.Repositories;

public interface ISiteRepository : IBaseRepository<Site>
{
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
    Task<bool> ExistsByAddressAsync(string address, int? excludeId = null);
    Task<bool> ExistsByPhoneAsync(string phone, int? excludeId = null);
    
    Task<Site?> FindByNameAsync(string name);
}