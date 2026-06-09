using System.Net.Mime;
using IceTrackPlatform.API.IAM.Domain.Model.Queries;
using IceTrackPlatform.API.IAM.Domain.Model.ValueObjects;
using IceTrackPlatform.API.IAM.Domain.Services;
using IceTrackPlatform.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using IceTrackPlatform.API.IAM.Interfaces.REST.Resources;
using IceTrackPlatform.API.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.IAM.Interfaces.REST;

/// <summary>
///     Controller that exposes endpoints to work with users.
/// </summary>
/// <remarks>
///     Routes are rooted at <c>api/v1/users</c>. Endpoints require authorization unless marked with
///     <see
///         cref="Attributes.AllowAnonymousAttribute" />
///     .
/// </remarks>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available User endpoints")]
public class UsersController(IUserQueryService userQueryService) : ControllerBase
{
    /// <summary>
    ///     Get a user by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the user to retrieve.</param>
    /// <returns>Returns an <see cref="IActionResult" /> that contains the <see cref="UserResource" /> when found.</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get a user by its id",
        Description = "Get a user by its id",
        OperationId = "GetUserById")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was found", typeof(UserResource))]
    public async Task<IActionResult> GetUserById(int id)
    {
        var getUserByIdQuery = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    /// <summary>
    ///     Get all users.
    /// </summary>
    /// <returns>Returns an <see cref="IActionResult" /> that contains a collection of <see cref="UserResource" /> objects.</returns>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all users",
        Description = "Get all users",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
    
    /// <summary>
    ///     Get users by role.
    /// </summary>
    /// <param name="role">The role to filter users by.</param>
    /// <returns>Returns an <see cref="IActionResult" /> that contains a collection of <see cref="UserResource" /> objects.</returns>
    [HttpGet("role/{role}")]
    [SwaggerOperation(
        Summary = "Get users by role",
        Description = "Get users by role",
        OperationId = "GetUsersByRole")]
    [SwaggerResponse(StatusCodes.Status200OK, "The users were found", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetUsersByRole(string role)
    {
        if (!Enum.TryParse<Roles>(role, true, out var userRole))
        {
            return BadRequest($"Invalid role: {role}");
        }
        var getUserByRoleQuery = new GetUserByRoleQuery(userRole);
        var users = await userQueryService.Handle(getUserByRoleQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }
}