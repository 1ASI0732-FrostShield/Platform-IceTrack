using IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Commands;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.ServiceRequests.Application.Internal.CommandServices;

public class ServiceRequestCommandService(
    IServiceRequestRepository serviceRequestRepository,
    IUnitOfWork unitOfWork) : IServiceRequestCommandService
{
    public async Task<ServiceRequest?> Handle(CreateServiceRequestCommand command)
    {
        var serviceRequest = new ServiceRequest(
            command.RequesterId,
            command.SiteId,
            command.EquipmentId,
            command.AssignedTo,
            command.Origin,
            command.Type,
            command.Priority,
            command.Description,
            command.OwnerId
            );
        
        await serviceRequestRepository.AddAsync(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }

    public async Task<ServiceRequest?> Handle(AcceptServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return null;
        serviceRequest.Accept();
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }

    public async Task<ServiceRequest?> Handle(RejectServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return null;
        serviceRequest.Reject();
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }

    public async Task<ServiceRequest?> Handle(CancelServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return null;
        serviceRequest.Cancel();
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }

    public async Task<ServiceRequest?> Handle(AssignTechnicianToServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return null;
        serviceRequest.AssignTechnician(command.TechnicianId);
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }

    public async Task<ServiceRequest?> Handle(CompleteServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return null;
        serviceRequest.Complete();
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return serviceRequest;
    }
    
    public async Task<bool> Handle(DeleteServiceRequestCommand command)
    {
        var serviceRequest = await serviceRequestRepository.FindByIdAsync(command.ServiceRequestId);
        if (serviceRequest == null) return false;

        serviceRequest.SoftDelete();
        serviceRequestRepository.Update(serviceRequest);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
