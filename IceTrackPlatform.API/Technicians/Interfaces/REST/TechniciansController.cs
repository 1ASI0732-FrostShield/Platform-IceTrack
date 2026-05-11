using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;
using IceTrackPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using IceTrackPlatform.API.Technicians.Domain.Model.Commands;
using IceTrackPlatform.API.Technicians.Domain.Model.Queries;
using IceTrackPlatform.API.Technicians.Domain.Services;
using IceTrackPlatform.API.Technicians.Interfaces.REST.Resources;
using IceTrackPlatform.API.Technicians.Interfaces.REST.Transform;

namespace IceTrackPlatform.API.Technicians.Interfaces.REST;

using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Technicians")]
public class TechniciansController(
    ITechnicianCommandService technicianCommandService,
    ITechnicianQueryService technicianQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a Technician",
        Description = "Creates a new Technician",
        OperationId = "CreateTechnician")]
    [SwaggerResponse(StatusCodes.Status201Created, "The technician was created", typeof(TechnicianResource))]
    public async Task<IActionResult> CreateTechnician([FromBody] CreateTechnicianResource resource)
    {
        var command = CreateTechnicianCommandFromResourceAssembler.ToCommandFromResource(resource);
        var technician = await technicianCommandService.Handle(command);
        if (technician == null) return BadRequest();
        var technicianResource = TechnicianResourceFromEntityAssembler.ToResourceFromEntity(technician);
        return CreatedAtAction(nameof(GetTechnicianById), new { technicianId = technicianResource.Id }, technicianResource);
    }

    [HttpGet("{technicianId:int}")]
    [SwaggerOperation(
        Summary = "Get Technician by Id",
        Description = "Gets a Technician by its Id",
        OperationId = "GetTechnicianById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The technician was found", typeof(TechnicianResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The technician was not found")]
    public async Task<IActionResult> GetTechnicianById(int technicianId)
    {
        var query = new GetTechnicianByIdQuery(technicianId);
        var technician = await technicianQueryService.Handle(query);
        if (technician == null) return NotFound();
        var technicianResource = TechnicianResourceFromEntityAssembler.ToResourceFromEntity(technician);
        return Ok(technicianResource);
    }
    
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get Technicians by Provider Id",
        Description = "Gets all Technicians for a given Provider Id",
        OperationId = "GetTechniciansByProviderId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The technicians were found", typeof(IEnumerable<TechnicianResource>))]
    public async Task<IActionResult> GetTechniciansByProviderId([FromQuery] int providerId)
    {
        var query = new GetTechniciansByProviderIdQuery(providerId);
        var technicians = await technicianQueryService.Handle(query);
        var technicianResources = technicians.Select(TechnicianResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(technicianResources);
    }

    [HttpPut("{technicianId:int}")]
    [SwaggerOperation(
        Summary = "Update a Technician",
        Description = "Updates a Technician by its Id",
        OperationId = "UpdateTechnician")]
    [SwaggerResponse(StatusCodes.Status200OK, "The technician was updated", typeof(TechnicianResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The technician was not found")]
    public async Task<IActionResult> UpdateTechnician(int technicianId, [FromBody] UpdateTechnicianResource resource)
    {
        var command = UpdateTechnicianCommandFromResourceAssembler.ToCommandFromResource(technicianId, resource);
        var technician = await technicianCommandService.Handle(command);
        if (technician == null) return NotFound();
        var technicianResource = TechnicianResourceFromEntityAssembler.ToResourceFromEntity(technician);
        return Ok(technicianResource);
    }

    [HttpDelete("{technicianId:int}")]
    [SwaggerOperation(
        Summary = "Delete a Technician",
        Description = "Deletes a Technician by its Id",
        OperationId = "DeleteTechnician")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The technician was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The technician was not found")]
    public async Task<IActionResult> DeleteTechnician(int technicianId)
    {
        var command = new DeleteTechnicianCommand(technicianId);
        await technicianCommandService.Handle(command);
        return NoContent();
    }
}
