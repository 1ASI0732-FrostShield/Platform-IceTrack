using System.Net.Mime;
using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.Queries;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;
using IceTrackPlatform.API.Monitoring.Domain.Services;
using IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;
using IceTrackPlatform.API.Monitoring.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.Monitoring.Interfaces.REST;

/// <summary>
///     Equipment REST API controller.
/// </summary>
/// <param name="equipmentCommandService">The Equipment Command Service</param>
/// <param name="equipmentQueryServices">The Equipment Query Service</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Equipment")]
public class EquipmentController(
    IEquipmentCommandService equipmentCommandService,
    IEquipmentQueryServices equipmentQueryServices)
    : ControllerBase
{
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a Equipment by Id", Description = "Gets a Equipment for a given identifier", OperationId = "GetEquipmentById")]
    [SwaggerResponse(200, "The equipment was found", typeof(EquipmentResource))]
    public async Task<IActionResult> GetEquipmentById(int id)
    {
        var getEquipmentByIdQuery = new GetEquipmentByIdQuery(id);
        var result = await equipmentQueryServices.Handle(getEquipmentByIdQuery);
        if (result is null) return NotFound();
        var resource = EquipmentResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all equipments", Description = "Gets the complete list of equipments", OperationId = "GetAllEquipments")]
    [SwaggerResponse(200, "List of equipments", typeof(IEnumerable<EquipmentResource>))]
    public async Task<IActionResult> GetAllEquipments()
    {
        var query = new GetAllEquipmentQuery();
        var results = await equipmentQueryServices.Handle(query);

        var resources = results
            .Select(EquipmentResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        return Ok(resources);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Equipment", Description = "Create a new Equipment", OperationId = "CreateEquipment")]
    [SwaggerResponse(201, "Created equipment", typeof(EquipmentResource))]
    [SwaggerResponse(400, "The equipment was not created")]
    [SwaggerResponse(409, "The equipment already exists")]
    public async Task<IActionResult> CreateEquipment(
        [FromBody] CreateEquipmentResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createEquipmentCommand = CreateEquipmentCommandFromResourceAssembler.ToCommandFromResource(resource);
        try
        {
            var result = await equipmentCommandService.Handle(createEquipmentCommand);
            if (result is null) return BadRequest();
            return CreatedAtAction(nameof(GetEquipmentById), new { id = result.Id },
                EquipmentResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (Exception e) when (e.Message.Contains("already exists"))
        {
            return Conflict("A equipment with the same properties already exists.");
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPut("{id:int}")]
    [SwaggerOperation(Summary = "Update Equipment", Description = "Updates an existing equipment", OperationId = "UpdateEquipment")]
    [SwaggerResponse(200, "Equipment updated", typeof(EquipmentResource))]
    [SwaggerResponse(400, "Bad request")]
    [SwaggerResponse(404, "Equipment not found")]
    public async Task<IActionResult> UpdateEquipment(int id, [FromBody] UpdateEquipmentResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var command = UpdateEquipmentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await equipmentCommandService.Handle(command);
        if (result is null) return NotFound($"Equipment with ID {id} not found.");
        return Ok(EquipmentResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
    
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Delete Equipment",
        Description = "Deletes a site.",
        OperationId = "DeleteEquipment")]
    [SwaggerResponse(204, "Equipment deleted.")]
    [SwaggerResponse(404, "Equipment not found.")]
    public async Task<IActionResult> DeleteEquipment(int id)
    {
        var command = new DeleteEquipmentCommand(id);
        var result = await equipmentCommandService.Handle(command);

        if (!result)
            return NotFound($"Equipment with ID {id} not found.");

        return NoContent();
    }
}