using IceTrackPlatform.API.Shared.Domain.Repositories;
using IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;
using IceTrackPlatform.API.Technicians.Domain.Model.Commands;
using IceTrackPlatform.API.Technicians.Domain.Repositories;
using IceTrackPlatform.API.Technicians.Domain.Services;

namespace IceTrackPlatform.API.Technicians.Application.Internal.CommandServices;



public class TechnicianCommandService(
    ITechnicianRepository technicianRepository,
    IUnitOfWork unitOfWork) : ITechnicianCommandService
{
    public async Task<Technician?> Handle(CreateTechnicianCommand command)
    {
        var technician = new Technician(command.Name, command.Specialty, command.Phone, command.ProviderId);
        await technicianRepository.AddAsync(technician);
        await unitOfWork.CompleteAsync();
        return technician;
    }

    public async Task<Technician?> Handle(UpdateTechnicianCommand command)
    {
        var technician = await technicianRepository.FindByIdAsync(command.Id);
        if (technician == null) return null;
        technician.Update(command.Name, command.Specialty, command.Phone);
        technicianRepository.Update(technician); 
        await unitOfWork.CompleteAsync();
        return technician;
    }

    public async Task Handle(DeleteTechnicianCommand command)
    {
        var technician = await technicianRepository.FindByIdAsync(command.Id);
        if (technician == null) return;
        technician.SoftDelete();
        technicianRepository.Update(technician);
        await unitOfWork.CompleteAsync();
    }
}
