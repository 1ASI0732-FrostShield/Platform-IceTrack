using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Model.Queries;
using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.Monitoring.Domain.Services;

namespace IceTrackPlatform.API.Monitoring.Application.Internal.QueryServices;

/// <summary>
///     Equipment Query Service
/// </summary>
/// <remarks>
///     This class handles queries related to new equipment.
///     It interacts with the IEquipmentRepository to retrieve data.
/// </remarks>
/// <param name="equipmentRepository"></param>
public class EquipmentQueryService(IEquipmentRepository equipmentRepository)
    : IEquipmentQueryServices
{
    public async Task<Equipment?> Handle(GetEquipmentByIdQuery query)
    {
        var equipment = await equipmentRepository.FindByIdAsync(query.Id);
        if (equipment?.DeletedAt != null) return null;
        return equipment;
    }

    public async Task<IEnumerable<Equipment?>> Handle(GetAllEquipmentQuery query)
    {
        var equipments = await equipmentRepository.ListAsync();
        return equipments.Where(e => e.DeletedAt == null);
    }
}