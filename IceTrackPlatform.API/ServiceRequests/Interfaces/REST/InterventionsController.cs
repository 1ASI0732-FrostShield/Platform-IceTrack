using System.Net.Mime;
using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;
using IceTrackPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Queries;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Interventions")]
public class InterventionsController(
    IInterventionCommandService interventionCommandService,
    IInterventionQueryService interventionQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Record an Intervention",
        Description = "Records a new Intervention for a Service Request",
        OperationId = "RecordIntervention")]
    [SwaggerResponse(StatusCodes.Status201Created, "The intervention was recorded", typeof(InterventionResource))]
    public async Task<IActionResult> RecordIntervention([FromBody] RecordInterventionResource resource)
    {
        var command = RecordInterventionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var intervention = await interventionCommandService.Handle(command);
        if (intervention == null) return BadRequest();
        var interventionResource = InterventionResourceFromEntityAssembler.ToResourceFromEntity(intervention);
        return CreatedAtAction(nameof(GetInterventionById), new { interventionId = interventionResource.Id }, interventionResource);
    }

    [HttpGet("{interventionId:int}")]
    [SwaggerOperation(
        Summary = "Get Intervention by Id",
        Description = "Gets an Intervention by its Id",
        OperationId = "GetInterventionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The intervention was found", typeof(InterventionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The intervention was not found")]
    public async Task<IActionResult> GetInterventionById(int interventionId)
    {
        var query = new GetInterventionByIdQuery(interventionId);
        var intervention = await interventionQueryService.Handle(query);
        if (intervention == null) return NotFound();
        var interventionResource = InterventionResourceFromEntityAssembler.ToResourceFromEntity(intervention);
        return Ok(interventionResource);
    }
    
    [HttpGet("serviceRequest/{serviceRequestId:int}")]
    [SwaggerOperation(
        Summary = "Get Interventions by Service Request Id",
        Description = "Gets all Interventions for a given Service Request Id",
        OperationId = "GetInterventionsByServiceRequestId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The interventions were found", typeof(IEnumerable<InterventionResource>))]
    public async Task<IActionResult> GetInterventionsByServiceRequestId(int serviceRequestId)
    {
        var query = new GetInterventionsByServiceRequestIdQuery(serviceRequestId);
        var interventions = await interventionQueryService.Handle(query);
        var interventionResources = interventions.Select(iv => InterventionResourceFromEntityAssembler.ToResourceFromEntity(iv));
        return Ok(interventionResources);
    }
}
