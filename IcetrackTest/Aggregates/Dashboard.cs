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
    public void EmptyConstructor_WhenCalled_ShouldCreateDashboardConfigWithDefaults()
    {
        var dashboardConfig = new DashboardConfig();

        Assert.AreEqual(0, dashboardConfig.UserId);
        Assert.AreEqual("24h", dashboardConfig.DefaultTemperatureRange.Value);
        Assert.AreEqual(0, dashboardConfig.Cards.Count);
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

    [TestMethod]
    public void Constructor_WithCreateCommand_ShouldCreateDashboardConfig()
    {
        var command = new CreateDashboardConfigCommand(5, 100, "7d");

        var dashboardConfig = new DashboardConfig(command);

        Assert.AreEqual(5, dashboardConfig.UserId);
        Assert.AreEqual(100, dashboardConfig.DefaultSiteId);
        Assert.AreEqual("7d", dashboardConfig.DefaultTemperatureRange.Value);
        Assert.AreEqual(4, dashboardConfig.Cards.Count);
    }

    [TestMethod]
    public void RemoveCard_WithExistingCardId_ShouldRemoveCard()
    {
        var dashboardConfig = CreateValidDashboardConfig();
        var cardId = dashboardConfig.Cards.First().Id;

        dashboardConfig.RemoveCard(cardId);

        Assert.AreEqual(3, dashboardConfig.Cards.Count);
    }

    [TestMethod]
    public void RemoveCard_WithNonExistentCardId_ShouldNotChangeCards()
    {
        var dashboardConfig = CreateValidDashboardConfig();

        dashboardConfig.RemoveCard(999);

        Assert.AreEqual(4, dashboardConfig.Cards.Count);
    }

    [TestMethod]
    public void ReorderCards_WithValidCommand_ShouldUpdateCardOrders()
    {
        var dashboardConfig = CreateValidDashboardConfig();
        var cards = dashboardConfig.Cards.ToList();
        var cardOrders = new Dictionary<int, int>
        {
            { 0, 3 }
        };
        var command = new ReorderCardsCommand(1, cardOrders);

        dashboardConfig.ReorderCards(command);

        Assert.AreEqual(3, cards[0].Order);
    }

    [TestMethod]
    public void ReorderCards_WithNegativeOrder_ShouldThrowArgumentException()
    {
        var dashboardConfig = CreateValidDashboardConfig();
        var card = dashboardConfig.Cards.First();
        var command = new ReorderCardsCommand(1, new Dictionary<int, int> { { card.Id, -1 } });

        try
        {
            dashboardConfig.ReorderCards(command);
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            Assert.IsTrue(true);
        }
    }

    [TestMethod]
    public void UpdateCardVisibility_WithValidCardId_ShouldChangeVisibility()
    {
        var dashboardConfig = CreateValidDashboardConfig();
        var card = dashboardConfig.Cards.First();
        var initialVisibility = card.IsVisible;

        dashboardConfig.UpdateCardVisibility(card.Id, !initialVisibility);

        Assert.AreEqual(!initialVisibility, card.IsVisible);
    }

    [TestMethod]
    public void CreateDashboardConfigCommand_WithValidData_ShouldCreateCommand()
    {
        var cmd = new CreateDashboardConfigCommand(1, 10, "24h");

        Assert.AreEqual(1, cmd.UserId);
        Assert.AreEqual(10, cmd.DefaultSiteId);
        Assert.AreEqual("24h", cmd.DefaultTemperatureRangeValue);
    }

    [TestMethod]
    public void DeleteDashboardConfigCommand_WithValidId_ShouldCreateCommand()
    {
        var cmd = new DeleteDashboardConfigCommand(5);

        Assert.AreEqual(5, cmd.DashboardConfigId);
    }

    [TestMethod]
    public void RemoveCardFromDashboardCommand_WithValidData_ShouldCreateCommand()
    {
        var cmd = new RemoveCardFromDashboardCommand(1, 2);

        Assert.AreEqual(1, cmd.DashboardConfigId);
        Assert.AreEqual(2, cmd.CardId);
    }

    [TestMethod]
    public void ReorderCardsCommand_WithValidData_ShouldCreateCommand()
    {
        var orders = new Dictionary<int, int> { { 1, 0 }, { 2, 1 } };
        var cmd = new ReorderCardsCommand(1, orders);

        Assert.AreEqual(1, cmd.DashboardConfigId);
        Assert.AreEqual(2, cmd.CardOrders.Count);
    }

    [TestMethod]
    public void UpdateCardVisibilityCommand_WithValidData_ShouldCreateCommand()
    {
        var cmd = new UpdateCardVisibilityCommand(1, 2, false);

        Assert.AreEqual(1, cmd.DashboardConfigId);
        Assert.AreEqual(2, cmd.CardId);
        Assert.IsFalse(cmd.IsVisible);
    }

    [TestMethod]
    public void TemperatureRange_DefaultConstructor_ShouldSet24Hours()
    {
        var range = new TemperatureRange();

        Assert.AreEqual("24h", range.Value);
        Assert.AreEqual("Last 24 Hours", range.Label);
    }
}