using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;

namespace IceTrackPlatform.API.Monitoring.Domain.Services;

public interface IEquipmentCommandService
{
    /// <summary>
    ///  Handle the create equipment command
    /// </summary>
    /// <remarks>
    ///     This method processes the CreateEquipmentCommand to create a new Equipment entity.
    ///     It performs necessary validations and persists the entity to the data store.
    ///     If it does not exist, it creates a new Equipment and returns it; otherwise, it returns null.
    /// </remarks>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<Equipment?> Handle(CreateEquipmentCommand command);
    
    /// <summary>
    ///  Handle the delete equipment config command
    /// </summary>
    /// <remarks>
    ///     Returns true if the equipment was deleted; otherwise false.
    /// </remarks>
    Task<bool> Handle(DeleteEquipmentCommand command);
    
    /// <summary>
    ///  Handle the update equipment config command
    /// </summary>
    Task<Equipment?> Handle(UpdateEquipmentCommand command);
}