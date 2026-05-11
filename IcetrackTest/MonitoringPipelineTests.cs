using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using IceTrackPlatform.API.Monitoring.Interfaces.REST;
using IceTrackPlatform.API.Monitoring.Domain.Services;
using IceTrackPlatform.API.Monitoring.Domain.Model.Aggregates;
using IceTrackPlatform.API.Monitoring.Domain.Model.Commands;
using IceTrackPlatform.API.Monitoring.Domain.Model.Queries;
using IceTrackPlatform.API.Monitoring.Domain.Model.ValueObjects;
using IceTrackPlatform.API.Monitoring.Interfaces.REST.Resources;

namespace IcetrackTest.Monitoring;

[TestClass]
public class MonitoringPipelineTests
{
    private Mock<IEquipmentCommandService> _commandServiceMock;
    private Mock<IEquipmentQueryServices> _queryServiceMock;
    private EquipmentController _controller;

    [TestInitialize]
    public void Setup()
    {
        _commandServiceMock = new Mock<IEquipmentCommandService>();
        _queryServiceMock = new Mock<IEquipmentQueryServices>();
        _controller = new EquipmentController(_commandServiceMock.Object, _queryServiceMock.Object);
    }

    // Helper para crear instancia de Equipment usando el constructor de comando
    private Equipment CreateTestEquipment()
    {
        var command = new CreateEquipmentCommand("ModelX", "TypeA", "SN123", StatusEquipment.ACTIVE, "SiteName", 1, true);
        return new Equipment(command);
    }

    [TestMethod]
    public async Task QueryPipeline_GetById_ReturnsOk()
    {
        var equipment = CreateTestEquipment();
        _queryServiceMock.Setup(s => s.Handle(It.IsAny<GetEquipmentByIdQuery>()))
                         .ReturnsAsync(equipment);

        var result = await _controller.GetEquipmentById(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task IngestionPipeline_Create_Success_ReturnsCreated()
    {
        var resource = new CreateEquipmentResource("ModelX", "TypeA", "SN123", StatusEquipment.ACTIVE, "SiteName", 1, true);
        var equipment = CreateTestEquipment();
        
        _commandServiceMock.Setup(s => s.Handle(It.IsAny<CreateEquipmentCommand>()))
                           .ReturnsAsync(equipment);

        var result = await _controller.CreateEquipment(resource);

        Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
    }

    [TestMethod]
    public async Task IngestionPipeline_Create_Conflict_Returns409()
    {
        var resource = new CreateEquipmentResource("ModelX", "TypeA", "SN123", StatusEquipment.ACTIVE, "SiteName", 1, true);
        _commandServiceMock.Setup(s => s.Handle(It.IsAny<CreateEquipmentCommand>()))
                           .ThrowsAsync(new Exception("already exists"));

        var result = await _controller.CreateEquipment(resource);

        Assert.IsInstanceOfType(result, typeof(ConflictObjectResult));
    }

    [TestMethod]
    public async Task UpdatePipeline_ValidUpdate_ReturnsOk()
    {
        var resource = new UpdateEquipmentResource("ModelX", "TypeA", "SN123", StatusEquipment.ACTIVE, "SiteName", 1, true);
        var equipment = CreateTestEquipment();
        
        _commandServiceMock.Setup(s => s.Handle(It.IsAny<UpdateEquipmentCommand>()))
                           .ReturnsAsync(equipment);

        var result = await _controller.UpdateEquipment(1, resource);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task DeletionPipeline_Success_ReturnsNoContent()
    {
        _commandServiceMock.Setup(s => s.Handle(It.IsAny<DeleteEquipmentCommand>()))
                           .ReturnsAsync(true);

        var result = await _controller.DeleteEquipment(1);

        Assert.IsInstanceOfType(result, typeof(NoContentResult));
    }
}