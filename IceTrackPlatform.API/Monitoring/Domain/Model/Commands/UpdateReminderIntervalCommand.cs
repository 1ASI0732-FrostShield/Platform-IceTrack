namespace IceTrackPlatform.API.Monitoring.Domain.Model.Commands;

public record UpdateReminderIntervalCommand(int EquipmentId, int? ReminderIntervalDays);
