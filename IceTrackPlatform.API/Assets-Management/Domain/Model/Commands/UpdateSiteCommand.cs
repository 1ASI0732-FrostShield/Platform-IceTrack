namespace IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;

public record UpdateSiteCommand(
    int SiteId,
    string Name,
    string Address,
    string ContactName,
    string Phone
    );