using IceTrackPlatform.API.Assets_Management.Domain.Model.Aggregates;
using IceTrackPlatform.API.Assets_Management.Domain.Model.Commands;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class SiteTests
{
    private static CreateSiteCommand CreateValidSiteCommand()
    {
        return new CreateSiteCommand(
            Name: "Warehouse A",
            Address: "Av. Industrial 123",
            ContactName: "Carlos Lopez",
            Phone: "999111222",
            OwnerId: 1
        );
    }

    [TestMethod]
    public void Constructor_WithValidCommand_ShouldCreateSite()
    {
        var command = CreateValidSiteCommand();

        var site = new Site(command);

        Assert.AreEqual("Warehouse A", site.Name);
        Assert.AreEqual("Av. Industrial 123", site.Address);
        Assert.AreEqual("Carlos Lopez", site.ContactName);
        Assert.AreEqual("999111222", site.Phone);
        Assert.AreEqual(1, site.OwnerId);
        Assert.AreEqual(0, site.CantEquipment);
    }

    [TestMethod]
    public void UpdateInformation_WithNewValues_ShouldChangeProperties()
    {
        var site = new Site(CreateValidSiteCommand());

        site.UpdateInformation("New Name", "New Address", "New Contact", "000000000");

        Assert.AreEqual("New Name", site.Name);
        Assert.AreEqual("New Address", site.Address);
        Assert.AreEqual("New Contact", site.ContactName);
        Assert.AreEqual("000000000", site.Phone);
    }

    [TestMethod]
    public void IncrementCantEquipment_WhenCalled_ShouldIncreaseByOne()
    {
        var site = new Site(CreateValidSiteCommand());

        site.IncrementCantEquipment();

        Assert.AreEqual(1, site.CantEquipment);
    }

    [TestMethod]
    public void DecrementCantEquipment_WhenCountIsZero_ShouldNotGoNegative()
    {
        var site = new Site(CreateValidSiteCommand());

        site.DecrementCantEquipment();

        Assert.AreEqual(0, site.CantEquipment);
    }

    [TestMethod]
    public void DecrementCantEquipment_WhenCountIsPositive_ShouldDecreaseByOne()
    {
        var site = new Site(CreateValidSiteCommand());
        site.IncrementCantEquipment();
        site.IncrementCantEquipment();

        site.DecrementCantEquipment();

        Assert.AreEqual(1, site.CantEquipment);
    }

    [TestMethod]
    public void SoftDelete_WhenCalled_ShouldSetDeletedAt()
    {
        var site = new Site(CreateValidSiteCommand());

        site.SoftDelete();

        Assert.IsNotNull(site.DeletedAt);
    }
}
