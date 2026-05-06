using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;

namespace IceTrackPlatform.API.Assets_Management.Domain.Services;

public interface ISiteCommandService
{
    /// <summary>
    ///  Handle the create site command
    /// </summary>
    /// <remarks>
    ///     This method processes the CreateSiteCommand to create a new Site entity.
    ///     It performs necessary validations and persists the entity to the data store.
    ///     If it does not exist, it creates a new Site and returns it; otherwise, it returns null.
    /// </remarks>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<Site?> Handle(CreateSiteCommand command);
    
    /// <summary>
    ///  Handle the delete site config command
    /// </summary>
    /// <remarks>
    ///     Returns true if the site was deleted; otherwise false.
    /// </remarks>
    Task<bool> Handle(DeleteSiteCommand command);
    
    /// <summary>
    ///  Handle the update site config command
    /// </summary>
    Task<Site?> Handle(UpdateSiteCommand command);
}