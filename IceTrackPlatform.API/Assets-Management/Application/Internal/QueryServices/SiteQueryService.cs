using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Queries;
using IceTrackPlatform.API.Assets_Management.Domain.Repositories;
using IceTrackPlatform.API.Assets_Management.Domain.Services;

namespace IceTrackPlatform.API.Assets_Management.Application.Internal.QueryServices;

/// <summary>
///     Site Query Service
/// </summary>
/// <remarks>
///     This class handles queries related to favorite news sources.
///     It interacts with the ISiteQueryServices to retrieve data.
/// </remarks>
/// <param name="siteRepository"></param>
public class SiteQueryService(ISiteRepository siteRepository)
    : ISiteQueryServices
{
    public async Task<Site?> Handle(GetSiteByIdQuery query)
    {
        var site = await siteRepository.FindByIdAsync(query.Id);
        if (site?.DeletedAt != null) return null;
        return site;
    }

    public async Task<IEnumerable<Site>> Handle(GetAllSitesQuery query)
    {
        var sites = await siteRepository.ListAsync();
        return sites.Where(s => s.DeletedAt == null);
    }
}