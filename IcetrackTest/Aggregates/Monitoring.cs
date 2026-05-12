using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class Monitoring
{
    private static CreateEquipmentCommand CreateValidEquipmentCommand()
    {
        return new CreateEquipmentCommand(
            Model: "Cooler X200",
            Type: "Refrigerator",
            Serial: "SN-ICE-001",
            Status: StatusEquipment.ACTIVE,
            Name: "Main refrigerator",
            SiteId: 1,
            Online: true
        );
    }

    [TestMethod]
    public void Constructor_WithValidCommand_ShouldCreateEquipment()
    {
        var command = CreateValidEquipmentCommand();

        var equipment = new Equipment(command);

        Assert.AreEqual("Cooler X200", equipment.Model);
        Assert.AreEqual("Refrigerator", equipment.Type);
        Assert.AreEqual("SN-ICE-001", equipment.Serial);
        Assert.AreEqual(StatusEquipment.ACTIVE, equipment.Status);
        Assert.AreEqual("Main refrigerator", equipment.Name);
        Assert.AreEqual(1, equipment.SiteId);
        Assert.IsTrue(equipment.Online);
    }

    [TestMethod]
    public void Constructor_WithValidCommand_ShouldGenerateEquipmentId()
    {
        var command = CreateValidEquipmentCommand();

        var equipment = new Equipment(command);

        Assert.AreNotEqual(Guid.Empty, equipment.EquipmentId);
    }

    [TestMethod]
    public void Constructor_WithActiveStatus_ShouldSetStatusToActive()
    {
        var command = new CreateEquipmentCommand(
            Model: "Freezer A100",
            Type: "Freezer",
            Serial: "SN-ACTIVE-001",
            Status: StatusEquipment.ACTIVE,
            Name: "Active freezer",
            SiteId: 2,
            Online: true
        );

        var equipment = new Equipment(command);

        Assert.AreEqual(StatusEquipment.ACTIVE, equipment.Status);
    }

    [TestMethod]
    public void Constructor_WithMaintenanceStatus_ShouldSetStatusToMaintenance()
    {
        var command = new CreateEquipmentCommand(
            Model: "Cooler M300",
            Type: "Cooler",
            Serial: "SN-MAINT-001",
            Status: StatusEquipment.MAINTENANCE,
            Name: "Maintenance cooler",
            SiteId: 3,
            Online: false
        );

        var equipment = new Equipment(command);

        Assert.AreEqual(StatusEquipment.MAINTENANCE, equipment.Status);
        Assert.IsFalse(equipment.Online);
    }

    [TestMethod]
    public void Constructor_WithOfflineEquipment_ShouldSetOnlineToFalse()
    {
        var command = new CreateEquipmentCommand(
            Model: "Freezer OFF",
            Type: "Freezer",
            Serial: "SN-OFF-001",
            Status: StatusEquipment.OFF,
            Name: "Offline freezer",
            SiteId: 4,
            Online: false
        );

        var equipment = new Equipment(command);

        Assert.AreEqual(StatusEquipment.OFF, equipment.Status);
        Assert.IsFalse(equipment.Online);
    }
}