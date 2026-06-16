using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;
using IceTrackPlatform.API.Assets_Management.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Assets_Management.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform CreateSiteResource to CreateSiteCommand.
/// </summary>
public static class CreateSiteCommandFromResourceAssembler
{
    public static CreateSiteCommand ToCommandFromResource(CreateSiteResource resource, int ownerId) => 
        new CreateSiteCommand(
            resource.Name, 
            resource.Address, 
            resource.ContactName, 
            resource.Phone,
            ownerId
            );
}