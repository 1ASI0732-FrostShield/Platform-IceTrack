using System.Net.Mime;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Queries;
using IceTrackPlatform.API.Assets_Management.Domain.Services;
using IceTrackPlatform.API.Assets_Management.Interfaces.REST.Resources;
using IceTrackPlatform.API.Assets_Management.Interfaces.REST.Transform;
using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;
using IceTrackPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.Assets_Management.Interfaces.REST;

/// <summary>
///     Site REST API controller.
/// </summary>
/// <param name="siteCommandService">The Site Command Service</param>
/// <param name="siteQueryServices">The Site Query Service</param>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Site")]
public class SiteController(
    ISiteCommandService siteCommandService,
    ISiteQueryServices siteQueryServices)
    : ControllerBase
{
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Gets a Site by Id", Description = "Gets a Site for a given identifier", OperationId = "GetSiteById")]
    [SwaggerResponse(200, "The site was found", typeof(SiteResource))]
    public async Task<IActionResult> GetSiteById(int id)
    {
        var getSiteByIdQuery = new GetSiteByIdQuery(id);
        var result = await siteQueryServices.Handle(getSiteByIdQuery);
        if (result is null) return NotFound();
        var resource = SiteResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet]
    [SwaggerOperation(Summary = "Get all sites", Description = "Gets the complete list of sites", OperationId = "GetAllSites")]
    [SwaggerResponse(200, "List of sites", typeof(IEnumerable<SiteResource>))]
    public async Task<IActionResult> GetAllSites()
    {
        var query = new GetAllSitesQuery();
        var results = await siteQueryServices.Handle(query);

        var resources = results
            .Select(SiteResourceFromEntityAssembler.ToResourceFromEntity)
            .ToList();

        return Ok(resources);
    }
    
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Site", Description = "Create a new Site", OperationId = "CreateSite")]
    [SwaggerResponse(201, "Created site", typeof(SiteResource))]
    [SwaggerResponse(400, "The site was not created")]
    [SwaggerResponse(409, "The site already exists")]
    public async Task<IActionResult> CreateSite(
        [FromBody] CreateSiteResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var createSiteCommandFromResourceAssembler = CreateSiteCommandFromResourceAssembler.ToCommandFromResource(resource);
        try
        {
            var result = await siteCommandService.Handle(createSiteCommandFromResourceAssembler);
            if (result is null) return BadRequest();
            return CreatedAtAction(nameof(GetSiteById), new { id = result.Id },
                SiteResourceFromEntityAssembler.ToResourceFromEntity(result));
        }
        catch (Exception e) when (e.Message.Contains("already exists"))
        {
            return Conflict("A site with the same Contact Name already exists.");
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Update Site",
        Description = "Updates an existing site.",
        OperationId = "UpdateSite")]
    [SwaggerResponse(200, "Site updated.", typeof(SiteResource))]
    [SwaggerResponse(404, "Site not found.")]
    [SwaggerResponse(400, "Invalid request.")]
    public async Task<IActionResult> UpdateSite(int id, [FromBody] UpdateSiteResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = new UpdateSiteCommand(
            id,
            resource.Name,
            resource.Address,
            resource.ContactName,
            resource.Phone
        );

        var result = await siteCommandService.Handle(command);

        if (result is null)
            return NotFound($"Site with ID {id} not found.");

        return Ok(SiteResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
    
    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Delete Site",
        Description = "Deletes a site.",
        OperationId = "DeleteSite")]
    [SwaggerResponse(204, "Site deleted.")]
    [SwaggerResponse(404, "Site not found.")]
    public async Task<IActionResult> DeleteSite(int id)
    {
        var command = new DeleteSiteCommand(id);
        var result = await siteCommandService.Handle(command);

        if (!result)
            return NotFound($"Site with ID {id} not found.");

        return NoContent();
    }
}