using System.Net.Mime;
using IceTrackPlatform.API.IAM.Domain.Repositories;
using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;
using IceTrackPlatform.API.Technicians.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST;

[ApiController]
[Route("api/v1/equipments/{equipmentId:int}/maintenance-history")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Maintenance")]
public class MaintenanceController(
    IServiceRequestRepository serviceRequestRepository,
    IInterventionRepository interventionRepository,
    IUserRepository userRepository,
    ITechnicianRepository technicianRepository,
    IEquipmentRepository equipmentRepository) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get maintenance history for an equipment",
        Description = "Gets all service requests and interventions for a given equipment, with provider and technician names resolved",
        OperationId = "GetMaintenanceHistoryByEquipmentId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The maintenance history was found", typeof(MaintenanceHistoryResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The equipment was not found")]
    public async Task<IActionResult> GetMaintenanceHistoryByEquipmentId(int equipmentId)
    {
        var equipment = await equipmentRepository.FindByIdAsync(equipmentId);
        if (equipment == null || equipment.DeletedAt != null)
            return NotFound(new { message = $"Equipment with id {equipmentId} not found" });

        var serviceRequests = (await serviceRequestRepository.FindByEquipmentIdAsync(equipmentId)).ToList();

        var allInterventions = new List<global::IceTrackPlatform.API.ServiceRequests.Domain.Model.Aggregates.Intervention>();
        foreach (var sr in serviceRequests)
        {
            var interventions = await interventionRepository.FindByServiceRequestIdAsync(sr.Id);
            allInterventions.AddRange(interventions);
        }

        var technicianIds = serviceRequests
            .Select(sr => sr.TechnicianId?.Value)
            .Where(id => id.HasValue)
            .Select(id => id!.Value)
            .Concat(allInterventions.Select(iv => iv.TechnicianId.Value))
            .Distinct()
            .ToList();

        var providerIds = serviceRequests
            .Select(sr => sr.AssignedTo.Value)
            .Distinct()
            .ToList();

        var technicians = new List<global::IceTrackPlatform.API.Technicians.Domain.Model.Aggregates.Technician>();
        foreach (var id in technicianIds)
        {
            var tech = await technicianRepository.FindByIdAsync(id);
            if (tech != null) technicians.Add(tech);
        }

        var providers = new List<global::IceTrackPlatform.API.IAM.Domain.Model.Aggregates.User>();
        foreach (var id in providerIds)
        {
            var user = await userRepository.FindByIdAsync(id);
            if (user != null) providers.Add(user);
        }

        var srResources = serviceRequests.Select(sr =>
        {
            var providerName = providers.FirstOrDefault(p => p.Id == sr.AssignedTo.Value)?.Username;
            var technicianName = sr.TechnicianId != null
                ? technicians.FirstOrDefault(t => t.Id == sr.TechnicianId.Value)?.Name
                : null;
            return ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(sr, providerName, technicianName);
        }).ToList();

        var interventionResources = allInterventions.Select(iv =>
        {
            var technicianName = technicians.FirstOrDefault(t => t.Id == iv.TechnicianId.Value)?.Name;
            return InterventionResourceFromEntityAssembler.ToResourceFromEntity(iv, technicianName);
        }).ToList();

        return Ok(new MaintenanceHistoryResource(
            equipmentId,
            srResources,
            interventionResources));
    }
}
