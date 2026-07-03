using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST.Transform;

/// <summary>
///     Assembler to transform Equipment entity to EquipmentResource.
/// </summary>
public static class EquipmentResourceFromEntityAssembler
{
    public static EquipmentResource ToResourceFromEntity(Equipment entity) =>
        new EquipmentResource(
            entity.Id, 
            entity.OwnerId,
            entity.EquipmentId, 
            entity.Model, 
            entity.Type, 
            entity.Serial, 
            entity.Status,
            entity.Name, 
            entity.SiteId, 
            entity.Online, 
            entity.CreatedDate, 
            entity.UpdatedDate,
            entity.ReminderIntervalDays);
}