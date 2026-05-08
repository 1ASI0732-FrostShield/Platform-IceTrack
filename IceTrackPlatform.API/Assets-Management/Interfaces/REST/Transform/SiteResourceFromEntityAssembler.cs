using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Assets_Management.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform Site entity to SiteResource.
/// </summary>
public static class SiteResourceFromEntityAssembler
{
    public static SiteResource ToResourceFromEntity(Site entity) =>
        new SiteResource(entity.Id, 
            entity.Name, 
            entity.Address, 
            entity.ContactName, 
            entity.Phone, 
            entity.CantEquipment,
            entity.CreatedDate, 
            entity.UpdatedDate
            );
}