using IceTrackPlatform.API.Dashboard.Domain.Model.Aggregates;
using IceTrackPlatform.API.Dashboard.Domain.Model.Commands;
using IceTrackPlatform.API.Dashboard.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class Dashboard
{
    private static DashboardConfig CreateValidDashboardConfig()
    {
        return new DashboardConfig(
            userId: 1,
            defaultSiteId: 10,
            defaultTemperatureRangeValue: "24h"
        );
    }

    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateDashboardConfig()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        Assert.AreEqual(1, dashboardConfig.UserId);
        Assert.AreEqual(10, dashboardConfig.DefaultSiteId);
        Assert.AreEqual("24h", dashboardConfig.DefaultTemperatureRange.Value);
        Assert.AreEqual("Last 24 Hours", dashboardConfig.DefaultTemperatureRange.Label);
    }

    [TestMethod]
    public void Constructor_WithValidData_ShouldInitializeDefaultCards()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        Assert.AreEqual(4, dashboardConfig.Cards.Count);
        Assert.IsTrue(dashboardConfig.Cards.Any(card => card.CardType == CardType.MonitoredEquipment));
        Assert.IsTrue(dashboardConfig.Cards.Any(card => card.CardType == CardType.OpenAlerts));
        Assert.IsTrue(dashboardConfig.Cards.Any(card => card.CardType == CardType.AverageTemperature));
        Assert.IsTrue(dashboardConfig.Cards.Any(card => card.CardType == CardType.RecentReports));
    }

    [TestMethod]
    public void Constructor_WithSevenDaysRange_ShouldSetDefaultTemperatureRangeToLastSevenDays()
    {
        var dashboardConfig = new DashboardConfig(
            userId: 2,
            defaultSiteId: 20,
            defaultTemperatureRangeValue: "7d"
        );

        Assert.AreEqual("7d", dashboardConfig.DefaultTemperatureRange.Value);
        Assert.AreEqual("Last 7 Days", dashboardConfig.DefaultTemperatureRange.Label);
    }

    [TestMethod]
    public void AddCard_WithNewCardType_ShouldAddCardToDashboard()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        var command = new AddCardToDashboardCommand(
            DashboardConfigId: 1,
            CardType: CardType.SystemHealth,
            Order: 4,
            IsVisible: true
        );

        dashboardConfig.AddCard(command);

        Assert.AreEqual(5, dashboardConfig.Cards.Count);
        Assert.IsTrue(dashboardConfig.Cards.Any(card =>
            card.CardType == CardType.SystemHealth &&
            card.Order == 4 &&
            card.IsVisible));
    }

    [TestMethod]
    public void AddCard_WithDuplicatedCardType_ShouldThrowInvalidOperationException()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        var command = new AddCardToDashboardCommand(
            DashboardConfigId: 1,
            CardType: CardType.OpenAlerts,
            Order: 4,
            IsVisible: true
        );

        try
        {
            dashboardConfig.AddCard(command);
            Assert.Fail("Expected InvalidOperationException was not thrown.");
        }
        catch (InvalidOperationException)
        {
            Assert.IsTrue(true);
        }
    }

    [TestMethod]
    public void Update_WithValidCommand_ShouldChangeDefaultSiteAndTemperatureRange()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        var command = new UpdateDashboardConfigCommand(
            DashboardConfigId: 1,
            DefaultSiteId: 30,
            DefaultTemperatureRangeValue: "30d"
        );

        dashboardConfig.Update(command);

        Assert.AreEqual(30, dashboardConfig.DefaultSiteId);
        Assert.AreEqual("30d", dashboardConfig.DefaultTemperatureRange.Value);
        Assert.AreEqual("Last 30 Days", dashboardConfig.DefaultTemperatureRange.Label);
    }
}