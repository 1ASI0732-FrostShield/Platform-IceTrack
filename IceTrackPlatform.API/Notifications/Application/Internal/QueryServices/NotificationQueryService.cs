using IceTrackPlatform.API.Monitoring.Domain.Repositories;
using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;
using IceTrackPlatform.API.Notifications.Domain.Repositories;
using IceTrackPlatform.API.Notifications.Domain.Services;
using IceTrackPlatform.API.ServiceRequests.Domain.Repositories;
using IceTrackPlatform.API.Shared.Domain.Repositories;

namespace IceTrackPlatform.API.Notifications.Application.Internal.QueryServices;

public class NotificationQueryService(
    INotificationRepository notificationRepository,
    IEquipmentRepository equipmentRepository,
    IServiceRequestRepository serviceRequestRepository,
    IUnitOfWork unitOfWork) : INotificationQueryService
{
    public async Task<IEnumerable<Notification>> HandleGetAllActive()
    {
        await GenerateOverdueNotifications();
        return await notificationRepository.FindAllActiveAsync();
    }

    public async Task<IEnumerable<Notification>> HandleGetByEquipmentId(int equipmentId)
    {
        var equipment = await equipmentRepository.FindByIdAsync(equipmentId);
        if (equipment == null || equipment.DeletedAt != null)
            return Enumerable.Empty<Notification>();

        await GenerateOverdueNotificationsForEquipment(equipment);
        return await notificationRepository.FindActiveByEquipmentIdAsync(equipmentId);
    }

    private async Task GenerateOverdueNotifications()
    {
        var allEquipment = await equipmentRepository.ListAsync();
        var activeEquipment = allEquipment.Where(e => e.DeletedAt == null && e.ReminderIntervalDays > 0);

        foreach (var equipment in activeEquipment)
        {
            await GenerateOverdueNotificationsForEquipment(equipment);
        }
    }

    private async Task GenerateOverdueNotificationsForEquipment(global::IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates.Equipment equipment)
    {
        if (equipment.ReminderIntervalDays == null || equipment.ReminderIntervalDays <= 0) return;

        var existingNotifications = await notificationRepository.FindActiveByEquipmentIdAsync(equipment.Id);
        if (existingNotifications.Any()) return;

        var lastServiceRequests = await serviceRequestRepository.FindByEquipmentIdAsync(equipment.Id);
        var lastCompleted = lastServiceRequests
            .Where(sr => sr.CompletedAt.HasValue && sr.DeletedAt == null)
            .OrderByDescending(sr => sr.CompletedAt)
            .FirstOrDefault();

        DateTime? lastMaintenanceDate = lastCompleted?.CompletedAt ?? equipment.CreatedDate?.UtcDateTime;

        if (lastMaintenanceDate == null) return;

        var nextReminderDate = lastMaintenanceDate.Value.AddDays(equipment.ReminderIntervalDays.Value);
        if (DateTime.UtcNow >= nextReminderDate)
        {
            var notification = new Notification(
                equipment.Id,
                $"Mantenimiento vencido para {equipment.Model} ({equipment.Serial}) — último mantenimiento: {lastMaintenanceDate.Value:dd/MM/yyyy}",
                "maintenance_reminder"
            );
            await notificationRepository.AddAsync(notification);
            await unitOfWork.CompleteAsync();
        }
    }
}
