namespace IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;

/// <summary>
///     Command to create a FavoriteSource aggregate
/// </summary>
/// <param name="Name">The Name obtained from the site</param>
/// <param name="Address">The Address of the site</param>
/// <param name="ContactName">The ContactName of the site</param>
/// <param name="Phone">The Phone of the site</param>
public record CreateSiteCommand(
    string Name, 
    string Address, 
    string ContactName, 
    string Phone,
    int OwnerId
    );