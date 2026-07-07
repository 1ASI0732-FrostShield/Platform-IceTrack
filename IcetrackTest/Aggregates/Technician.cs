using IceTrackPlatform.API.Technicians.Domain.Model.Aggregates;
using IceTrackPlatform.API.Technicians.Domain.Model.ValueObjects;

namespace IceTrackPlatform.Tests.Aggregates;

[TestClass]
public class TechnicianTests
{
    [TestMethod]
    public void Constructor_WithValidData_ShouldCreateTechnician()
    {
        var technician = new Technician(
            name: "Juan Perez",
            specialty: "Refrigeration",
            phone: "999888777",
            providerId: 1
        );

        Assert.AreEqual("Juan Perez", technician.Name);
        Assert.AreEqual("Refrigeration", technician.Specialty);
        Assert.AreEqual("999888777", technician.Phone);
        Assert.AreEqual(1, technician.ProviderId.Value);
    }

    [TestMethod]
    public void EmptyConstructor_WhenCalled_ShouldCreateDefaultTechnician()
    {
        var technician = new Technician();

        Assert.AreEqual(string.Empty, technician.Name);
        Assert.AreEqual(string.Empty, technician.Specialty);
        Assert.AreEqual(string.Empty, technician.Phone);
        Assert.AreEqual(0, technician.ProviderId.Value);
    }

    [TestMethod]
    public void Update_WithNewValues_ShouldChangeAllProperties()
    {
        var technician = new Technician("Old Name", "Old Specialty", "000000000", 1);

        technician.Update("New Name", "New Specialty", "111111111");

        Assert.AreEqual("New Name", technician.Name);
        Assert.AreEqual("New Specialty", technician.Specialty);
        Assert.AreEqual("111111111", technician.Phone);
    }

    [TestMethod]
    public void SoftDelete_WhenCalled_ShouldSetDeletedAt()
    {
        var technician = new Technician("Test", "Test", "000000000", 1);

        technician.SoftDelete();

        Assert.IsNotNull(technician.DeletedAt);
    }
}
