using System.Net.Mime;
using IceTrackPlatform.API.Dashboard.Domain.Model.Commands;
using IceTrackPlatform.API.Dashboard.Domain.Model.Queries;
using IceTrackPlatform.API.Dashboard.Domain.Model.ValueObjects;
using IceTrackPlatform.API.Dashboard.Domain.Services;
using IceTrackPlatform.API.Dashboard.Interfaces.REST.Resources;
using IceTrackPlatform.API.Dashboard.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.Dashboard.Interfaces.REST;

/// <summary>
///     Controller for managing dashboard configurations.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Dashboard Configuration Endpoints.")]
public class DashboardConfigsController(
    IDashboardConfigCommandService commandService,
    IDashboardConfigQueryService queryService)
    : ControllerBase
{
    /// <summary>
    ///     Gets a dashboard configuration by ID.
    /// </summary>
    /// <param name="id">The dashboard configuration ID.</param>
    /// <returns>The dashboard configuration resource.</returns>
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Get Dashboard Config by ID",
        Description = "Retrieves a dashboard configuration by its unique identifier.",
        OperationId = "GetDashboardConfigById")]
    [SwaggerResponse(200, "Dashboard configuration found.", typeof(DashboardConfigResource))]
    [SwaggerResponse(404, "Dashboard configuration not found.")]
    public async Task<IActionResult> GetDashboardConfigById(int id)
    {
        var query = new GetDashboardConfigByIdQuery(id);
        var result = await queryService.Handle(query);
        if (result is null) return NotFound();
        var resource = DashboardConfigResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Gets a dashboard configuration by user ID.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <returns>The dashboard configuration resource.</returns>
    [HttpGet("user/{userId:int}")]
    [SwaggerOperation(
        Summary = "Get Dashboard Config by User ID",
        Description = "Retrieves a dashboard configuration for a specific user.",
        OperationId = "GetDashboardConfigByUserId")]
    [SwaggerResponse(200, "Dashboard configuration found.", typeof(DashboardConfigResource))]
    [SwaggerResponse(404, "Dashboard configuration not found.")]
    public async Task<IActionResult> GetDashboardConfigByUserId(int userId)
    {
        var query = new GetDashboardConfigByUserIdQuery(userId);
        var result = await queryService.Handle(query);
        if (result is null) return NotFound();
        var resource = DashboardConfigResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    /// <summary>
    ///     Creates a new dashboard configuration.
    /// </summary>
    /// <param name="resource">The create dashboard config resource.</param>
    /// <returns>The created dashboard configuration.</returns>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Dashboard Config",
        Description = "Creates a new dashboard configuration for a user.",
        OperationId = "CreateDashboardConfig")]
    [SwaggerResponse(201, "Dashboard configuration created.", typeof(DashboardConfigResource))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(409, "Dashboard configuration already exists for this user.")]
    public async Task<IActionResult> CreateDashboardConfig([FromBody] CreateDashboardConfigResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = CreateDashboardConfigCommandFromResourceAssembler.ToCommandFromResource(resource);

        try
        {
            var result = await commandService.Handle(command);
            if (result is null)
                return BadRequest("Failed to create dashboard configuration.");

            var dashboardResource = DashboardConfigResourceFromEntityAssembler.ToResourceFromEntity(result);
            return CreatedAtAction(
                nameof(GetDashboardConfigById),
                new { id = result.Id },
                dashboardResource);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
   
    /// <summary>
    ///     Adds a card to a dashboard.
    /// </summary>
    /// <param name="id">The dashboard configuration ID.</param>
    /// <param name="resource">The add card resource.</param>
    /// <returns>The updated dashboard configuration.</returns>
    [HttpPost("{id:int}/cards")]
    [SwaggerOperation(
        Summary = "Add Card to Dashboard",
        Description = "Adds a new card to the dashboard configuration.",
        OperationId = "AddCardToDashboard")]
    [SwaggerResponse(200, "Card added successfully.", typeof(DashboardConfigResource))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(404, "Dashboard configuration not found.")]
    public async Task<IActionResult> AddCardToDashboard(int id, [FromBody] AddCardResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (!Enum.TryParse<CardType>(resource.CardType, out var cardType))
            return BadRequest($"Invalid card type: {resource.CardType}");

        var command = new AddCardToDashboardCommand(id, cardType, resource.Order, resource.IsVisible);

        try
        {
            var result = await commandService.Handle(command);
            if (result is null)
                return NotFound($"Dashboard configuration with ID {id} not found.");

            var dashboardResource = DashboardConfigResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Ok(dashboardResource);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    ///     Updates the visibility of a card.
    /// </summary>
    /// <param name="id">The dashboard configuration ID.</param>
    /// <param name="cardId">The card ID.</param>
    /// <param name="resource">The update visibility resource.</param>
    /// <returns>The updated dashboard configuration.</returns>
    [HttpPatch("{id:int}/cards/{cardId:int}/visibility")]
    [SwaggerOperation(
        Summary = "Update Card Visibility",
        Description = "Updates the visibility state of a specific card.",
        OperationId = "UpdateCardVisibility")]
    [SwaggerResponse(200, "Card visibility updated.", typeof(DashboardConfigResource))]
    [SwaggerResponse(400, "Invalid request data.")]
    [SwaggerResponse(404, "Dashboard configuration or card not found.")]
    public async Task<IActionResult> UpdateCardVisibility(
        int id,
        int cardId,
        [FromBody] UpdateCardVisibilityResource resource)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var command = new UpdateCardVisibilityCommand(id, cardId, resource.IsVisible);

        try
        {
            var result = await commandService.Handle(command);
            if (result is null)
                return NotFound($"Dashboard configuration with ID {id} not found.");

            var dashboardResource = DashboardConfigResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Ok(dashboardResource);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    ///     Gets all available dashboard card types.
    /// </summary>
    /// <returns>A list of available card types.</returns>
    [HttpGet("available-cards")]
    [SwaggerOperation(
        Summary = "Get Available Dashboard Cards",
        Description = "Retrieves all available dashboard card types.",
        OperationId = "GetAvailableDashboardCards")]
    [SwaggerResponse(200, "Available card types retrieved.", typeof(IEnumerable<string>))]
    public async Task<IActionResult> GetAvailableDashboardCards()
    {
        var query = new GetAvailableDashboardCardsQuery();
        var result = await queryService.Handle(query);
        var cardTypes = result.Select(ct => ct.ToString());
        return Ok(cardTypes);
    }
}