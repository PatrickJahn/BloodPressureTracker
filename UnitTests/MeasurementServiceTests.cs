using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MeasurementService.DTOs;
using MeasurementService.Models;
using MeasurementService.Repositories.Interfaces;
using Moq;
using Xunit;
namespace UnitTests;

public class MeasurementServiceTests
{
    private readonly Mock<IMeasurementRepository> _mockMeasurementRepository;
    private readonly IMapper _mapper;
    private readonly MeasurementService.Services.MeasurementService _measurementService;

    public MeasurementServiceTests()
    {
        _mockMeasurementRepository = new Mock<IMeasurementRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Measurement, MeasurementDto>();
            cfg.CreateMap<CreateMeasurementDto, Measurement>();
        });
        _mapper = config.CreateMapper();

        _measurementService = new MeasurementService.Services.MeasurementService(_mockMeasurementRepository.Object, _mapper);
    }
    
    [Fact]
    public async Task GetAllMeasurementsAsync_ShouldReturnMappedMeasurementDtos()
    {
        // Arrange
        var measurements = new List<Measurement>
        {
            new Measurement { Id = 1, Systolic = 120, Diastolic = 80, Seen = false, PatientSSN = "123-45-6789", Date = DateTime.UtcNow },
            new Measurement { Id = 2, Systolic = 130, Diastolic = 85, Seen = true, PatientSSN = "987-65-4321", Date = DateTime.UtcNow }
        };

        _mockMeasurementRepository.Setup(repo => repo.GetAllMeasurementsAsync())
            .ReturnsAsync(measurements);

        // Act
        var result = await _measurementService.GetAllMeasurementsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Count(), measurements.Count);
        _mockMeasurementRepository.Verify(repo => repo.GetAllMeasurementsAsync(), Times.Once);
    }
    [Fact]
    public async Task GetAllMeasurementsAsync_ShouldThrowArgumentNullException_WhenRepositoryReturnsNull()
    {
        // Arrange
        _mockMeasurementRepository.Setup(repo => repo.GetAllMeasurementsAsync())
            .ReturnsAsync((List<Measurement>)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _measurementService.GetAllMeasurementsAsync());
    }
    
    [Fact]
    public async Task GetMeasurementsBySSNAsync_ShouldReturnMappedMeasurementDtos()
    {
        // Arrange
        var ssn = "123-45-6789";
        var measurements = new List<Measurement>
        {
            new Measurement { Id = 1, Systolic = 120, Diastolic = 80, Seen = false, PatientSSN = ssn, Date = DateTime.UtcNow }
        };

        _mockMeasurementRepository.Setup(repo => repo.GetAllMeasurementsBySSNAsync(ssn))
            .ReturnsAsync(measurements);

        // Act
        var result = await _measurementService.GetMeasurementsBySSNAsync(ssn);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Count(), measurements.Count);
        _mockMeasurementRepository.Verify(repo => repo.GetAllMeasurementsBySSNAsync(ssn), Times.Once);
    }

    [Fact]
    public async Task GetMeasurementsBySSNAsync_ShouldThrowArgumentNullException_WhenRepositoryReturnsNull()
    {
        // Arrange
        var ssn = "123-45-6789";
        _mockMeasurementRepository.Setup(repo => repo.GetAllMeasurementsBySSNAsync(ssn))
            .ReturnsAsync((List<Measurement>)null);

        // Act & Assert
         await Assert.ThrowsAsync<ArgumentNullException>(() => _measurementService.GetMeasurementsBySSNAsync(ssn));
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ShouldReturnMappedMeasurementDto()
    {
        // Arrange
        var id = Guid.NewGuid();
        var measurement = new Measurement
        {
            Id = 1,
            Systolic = 120,
            Diastolic = 80,
            Seen = false,
            PatientSSN = "123-45-6789",
            Date = DateTime.UtcNow
        };

        _mockMeasurementRepository.Setup(repo => repo.GetMeasurementByIdAsync(id))
            .ReturnsAsync(measurement);

        // Act
        var result = await _measurementService.GetMeasurementByIdAsync(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Systolic, measurement.Systolic);
        _mockMeasurementRepository.Verify(repo => repo.GetMeasurementByIdAsync(id), Times.Once);
    }

    [Fact]
    public async Task GetMeasurementByIdAsync_ShouldThrowKeyNotFoundException_WhenMeasurementDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _mockMeasurementRepository.Setup(repo => repo.GetMeasurementByIdAsync(id))
            .ReturnsAsync((Measurement)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _measurementService.GetMeasurementByIdAsync(id));
    }

    [Fact]
    public async Task AddMeasurementAsync_ShouldAddMeasurementToRepository()
    {
        // Arrange
        var dto = new CreateMeasurementDto
        {
            Diastolic = 80,
            Systolic = 120,
            PatientSSn = "123-45-6789"
        };

        _mockMeasurementRepository.Setup(repo => repo.AddMeasurementAsync(It.IsAny<Measurement>()))
            .Returns(Task.CompletedTask);

        // Act
        await _measurementService.AddMeasurementAsync(dto);

        // Assert
        _mockMeasurementRepository.Verify(repo => repo.AddMeasurementAsync(It.Is<Measurement>(m =>
                m.Systolic == dto.Systolic &&
                m.Diastolic == dto.Diastolic &&
                m.PatientSSN == dto.PatientSSn &&
                m.Seen == false &&
                m.Date <= DateTime.UtcNow // Ensure date is set to now
        )), Times.Once);
    }
    [Fact]
    public async Task DeleteMeasurementAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _measurementService.DeleteMeasurementAsync(id);

        // Assert
        _mockMeasurementRepository.Verify(repo => repo.DeleteMeasurementAsync(id), Times.Once);
    }


}
