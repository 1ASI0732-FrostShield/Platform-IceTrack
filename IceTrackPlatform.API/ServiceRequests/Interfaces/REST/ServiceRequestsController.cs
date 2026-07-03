using System.Net.Mime;
using IceTrackPlatform.API.IAM.Domain.Model.Aggregates;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Commands;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.Queries;
using IceTrackPlatform.API.ServiceRequests.Domain.Model.ValueObjects;
using IceTrackPlatform.API.ServiceRequests.Domain.Services;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Resources;
using IceTrackPlatform.API.ServiceRequests.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.ServiceRequests.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Service Requests")]
public class ServiceRequestsController(
    IServiceRequestCommandService serviceRequestCommandService,
    IServiceRequestQueryService serviceRequestQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a Service Request",
        Description = "Creates a new Service Request",
        OperationId = "CreateServiceRequest")]
    [SwaggerResponse(StatusCodes.Status201Created, "The service request was created", typeof(ServiceRequestResource))]
    public async Task<IActionResult> CreateServiceRequest([FromBody] CreateServiceRequestResource resource)
    {
        var user = HttpContext.Items["User"] as User;
        if (user is null) return Unauthorized();

        var command = CreateServiceRequestCommandFromResourceAssembler.ToCommandFromResource(resource, user.Id);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return BadRequest();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return CreatedAtAction(nameof(GetServiceRequestById), new { serviceRequestId = serviceRequestResource.Id }, serviceRequestResource);
    }

    [HttpGet("{serviceRequestId:int}")]
    [SwaggerOperation(
        Summary = "Get Service Request by Id",
        Description = "Gets a Service Request by its Id",
        OperationId = "GetServiceRequestById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service request was found", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> GetServiceRequestById(int serviceRequestId)
    {
        var query = new GetServiceRequestByIdQuery(serviceRequestId);
        var serviceRequest = await serviceRequestQueryService.Handle(query);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpGet("requester/{requesterId:int}")]
    [SwaggerOperation(
        Summary = "Get Service Requests by Requester Id",
        Description = "Gets all Service Requests for a given Requester Id",
        OperationId = "GetServiceRequestsByRequesterId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service requests were found", typeof(IEnumerable<ServiceRequestResource>))]
    public async Task<IActionResult> GetServiceRequestsByRequesterId(int requesterId)
    {
        var query = new GetServiceRequestsByRequesterIdQuery(requesterId);
        var serviceRequests = await serviceRequestQueryService.Handle(query);
        var serviceRequestResources = serviceRequests.Select(sr => ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(sr));
        return Ok(serviceRequestResources);
    }

    [HttpGet("provider/{providerId:int}")]
    [SwaggerOperation(
        Summary = "Get Service Requests by Provider Id",
        Description = "Gets all Service Requests for a given Provider Id, optionally filtered by status",
        OperationId = "GetServiceRequestsByProviderId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service requests were found", typeof(IEnumerable<ServiceRequestResource>))]
    public async Task<IActionResult> GetServiceRequestsByProviderId(int providerId, [FromQuery] EServiceRequestStatus? status = null)
    {
        var query = new GetServiceRequestsByProviderIdQuery(providerId, status);
        var serviceRequests = await serviceRequestQueryService.Handle(query);
        var serviceRequestResources = serviceRequests.Select(sr => ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(sr));
        return Ok(serviceRequestResources);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Service Requests",
        Description = "Gets all Service Requests",
        OperationId = "GetAllServiceRequests")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service requests were found", typeof(IEnumerable<ServiceRequestResource>))]
    public async Task<IActionResult> GetAllServiceRequests()
    {
        var query = new GetAllServiceRequestsQuery();
        var serviceRequests = await serviceRequestQueryService.Handle(query);
        var serviceRequestResources = serviceRequests.Select(sr => ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(sr));

        return Ok(serviceRequestResources);
    }

    [HttpPatch("{serviceRequestId:int}/accept")]
    [SwaggerOperation(
        Summary = "Accept a Service Request",
        Description = "Accepts a Service Request by its Id",
        OperationId = "AcceptServiceRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service request was accepted", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> AcceptServiceRequest(int serviceRequestId)
    {
        var command = new AcceptServiceRequestCommand(serviceRequestId);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpPatch("{serviceRequestId:int}/reject")]
    [SwaggerOperation(
        Summary = "Reject a Service Request",
        Description = "Rejects a Service Request by its Id",
        OperationId = "RejectServiceRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service request was rejected", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> RejectServiceRequest(int serviceRequestId)
    {
        var command = new RejectServiceRequestCommand(serviceRequestId);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpPatch("{serviceRequestId:int}/cancel")]
    [SwaggerOperation(
        Summary = "Cancel a Service Request",
        Description = "Cancels a Service Request by its Id",
        OperationId = "CancelServiceRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service request was canceled", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> CancelServiceRequest(int serviceRequestId)
    {
        var command = new CancelServiceRequestCommand(serviceRequestId);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpPatch("{serviceRequestId:int}/assign-technician")]
    [SwaggerOperation(
        Summary = "Assign Technician to Service Request",
        Description = "Assigns a technician to a Service Request by its Id",
        OperationId = "AssignTechnicianToServiceRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The technician was assigned", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> AssignTechnicianToServiceRequest(int serviceRequestId, [FromBody] AssignTechnicianToServiceRequestResource resource)
    {
        var command = new AssignTechnicianToServiceRequestCommand(serviceRequestId, resource.TechnicianId);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpPatch("{serviceRequestId:int}/complete")]
    [SwaggerOperation(
        Summary = "Complete a Service Request",
        Description = "Completes a Service Request by its Id",
        OperationId = "CompleteServiceRequest")]
    [SwaggerResponse(StatusCodes.Status200OK, "The service request was completed", typeof(ServiceRequestResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> CompleteServiceRequest(int serviceRequestId)
    {
        var command = new CompleteServiceRequestCommand(serviceRequestId);
        var serviceRequest = await serviceRequestCommandService.Handle(command);
        if (serviceRequest == null) return NotFound();
        var serviceRequestResource = ServiceRequestResourceFromEntityAssembler.ToResourceFromEntity(serviceRequest);
        return Ok(serviceRequestResource);
    }
    
    [HttpDelete("{serviceRequestId:int}")]
    [SwaggerOperation(
        Summary = "Delete a Service Request",
        Description = "Deletes a Service Request by its Id",
        OperationId = "DeleteServiceRequest")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "The service request was deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The service request was not found")]
    public async Task<IActionResult> DeleteServiceRequest(int serviceRequestId)
    {
        var command = new DeleteServiceRequestCommand(serviceRequestId);
        var deleted = await serviceRequestCommandService.Handle(command);

        if (!deleted) return NotFound();

        return NoContent();
    }
}
