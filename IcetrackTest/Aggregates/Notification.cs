using IceTrackPlatform.API.Notifications.Domain.Model.Aggregates;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class NotificationTests
{
    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateNotification()
    {
        var notification = new Notification(
            equipmentId: 1,
            message: "Maintenance reminder for equipment Cooler X200",
            type: "maintenance_reminder"
        );

        Assert.AreEqual(1, notification.EquipmentId);
        Assert.AreEqual("Maintenance reminder for equipment Cooler X200", notification.Message);
        Assert.AreEqual("maintenance_reminder", notification.Type);
        Assert.IsFalse(notification.IsRead);
    }

    [TestMethod]
    public void EmptyConstructor_WhenCalled_ShouldCreateDefaultNotification()
    {
        var notification = new Notification();

        Assert.AreEqual(string.Empty, notification.Message);
        Assert.AreEqual("maintenance_reminder", notification.Type);
    }

    [TestMethod]
    public void MarkAsRead_WhenCalled_ShouldSetIsReadAndReadAt()
    {
        var notification = new Notification(1, "Test message", "alert");

        notification.MarkAsRead();

        Assert.IsTrue(notification.IsRead);
        Assert.IsNotNull(notification.ReadAt);
    }

    [TestMethod]
    public void Dismiss_WhenCalled_ShouldSetDismissedAt()
    {
        var notification = new Notification(1, "Test message", "alert");

        notification.Dismiss();

        Assert.IsNotNull(notification.DismissedAt);
    }
}
