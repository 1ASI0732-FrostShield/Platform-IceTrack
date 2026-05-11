using System.ComponentModel.DataAnnotations;

namespace IceTrackPlatform.API.Assets_Management.Interfaces.REST.Resources;

public record UpdateSiteResource(
    [Required] string Name,
    [Required] string Address,
    [Required] string ContactName,
    [Required] string Phone
    );