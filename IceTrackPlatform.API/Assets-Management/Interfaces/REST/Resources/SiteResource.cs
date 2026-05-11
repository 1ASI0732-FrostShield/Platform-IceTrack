namespace IceTrackPlatform.API.Assets_Management.Interfaces.REST.Resources;

/// <summary>
///     Represents the resource for a site.
/// </summary>
public record SiteResource(int Id, string Name, string Address, string ContactName, string Phone, int CantEquipment,
    DateTimeOffset? Created, DateTimeOffset? Updated);