using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Monitoring.Domain.Repositories;

/// <summary>
///     The Equipment Repository interface
/// </summary>
public interface IEquipmentRepository : IBaseRepository<Equipment>
{
}