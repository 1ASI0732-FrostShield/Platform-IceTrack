using IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;
using IceTrackPlatform.API.Technicians.Domain.Model.Queries;
using IceTrackPlatform.API.Technicians.Domain.Repositories;
using IceTrackPlatform.API.Technicians.Domain.Services;

namespace IceTrackPlatform.API.Technicians.Application.Internal.QueryServices;


public class TechnicianQueryService(ITechnicianRepository technicianRepository) : ITechnicianQueryService
{
    public async Task<Technician?> Handle(GetTechnicianByIdQuery query)
    {
        var technician = await technicianRepository.FindByIdAsync(query.Id);
        if (technician?.DeletedAt != null) return null;
        return technician;
    }

    public async Task<IEnumerable<Technician>> Handle(GetTechniciansByProviderIdQuery query)
    {
        var technicians = await technicianRepository.FindByProviderIdAsync(query.ProviderId);
        return technicians.Where(t => t.DeletedAt == null);
    }
}

