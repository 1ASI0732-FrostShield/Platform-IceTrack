using System.Net.Mime;
using IceTrackPlatform.API.Notifications.Domain.Repositories;
using IceTrackPlatform.API.Notifications.Domain.Services;
using IceTrackPlatform.API.Notifications.Interfaces.REST.Resources;
using IceTrackPlatform.API.Notifications.Interfaces.REST.Transform;
using IceTrackPlatform.API.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IceTrackPlatform.API.Notifications.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Notifications")]
public class NotificationsController(
    INotificationQueryService notificationQueryService,
    INotificationRepository notificationRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get all active notifications",
        Description = "Gets all non-dismissed notifications. Auto-generates overdue maintenance notifications.",
        OperationId = "GetAllActiveNotifications")]
    [SwaggerResponse(StatusCodes.Status200OK, "The active notifications were found", typeof(IEnumerable<NotificationResource>))]
    public async Task<IActionResult> GetAllActiveNotifications()
    {
        var notifications = await notificationQueryService.HandleGetAllActive();
        var resources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("equipment/{equipmentId:int}")]
    [SwaggerOperation(
        Summary = "Get active notifications by equipment",
        Description = "Gets all non-dismissed notifications for a given equipment. Auto-generates if overdue.",
        OperationId = "GetNotificationsByEquipmentId")]
    [SwaggerResponse(StatusCodes.Status200OK, "The notifications were found", typeof(IEnumerable<NotificationResource>))]
    public async Task<IActionResult> GetNotificationsByEquipmentId(int equipmentId)
    {
        var notifications = await notificationQueryService.HandleGetByEquipmentId(equipmentId);
        var resources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPatch("{notificationId:int}/dismiss")]
    [SwaggerOperation(
        Summary = "Dismiss a notification",
        Description = "Marks a notification as dismissed (soft-delete from active view)",
        OperationId = "DismissNotification")]
    [SwaggerResponse(StatusCodes.Status200OK, "The notification was dismissed", typeof(NotificationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "The notification was not found")]
    public async Task<IActionResult> DismissNotification(int notificationId)
    {
        var notification = await notificationRepository.FindByIdAsync(notificationId);
        if (notification == null) return NotFound();

        notification.Dismiss();
        notificationRepository.Update(notification);
        await unitOfWork.CompleteAsync();

        var resource = NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
        return Ok(resource);
    }
}
