using AutoMapper;
using Moq;
using PatientService.DTOs;
using PatientService.Models;
using PatientService.Repositories.Interfaces;
using Xunit;

namespace UnitTests;

public class PatientServiceTests
{
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly IMapper _mapper;
    private readonly PatientService.Services.PatientService _patientService;

    public PatientServiceTests()
    {
        _mockPatientRepository = new Mock<IPatientRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Patient, PatientDto>();
            cfg.CreateMap<CreatePatientDto, Patient>();
            cfg.CreateMap<UpdatePatientDto, Patient>();
        });
        _mapper = config.CreateMapper();

        _patientService = new PatientService.Services.PatientService(_mockPatientRepository.Object, _mapper);
    }
    
    [Fact]
    public async Task GetAllPatientsAsync_ShouldReturnMappedPatientDtos()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient
            {
                PatientId = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "jane@example.com",
                DateOfBirth = default,
                Gender = null
            },
            new Patient
            {
                PatientId = Guid.NewGuid(),
                Name = "John Smith",
                Email = "john@example.com",
                DateOfBirth = default,
                Gender = null
            }
        };

        _mockPatientRepository.Setup(repo => repo.GetAllPatientsAsync())
            .ReturnsAsync(patients);

        // Act
        var result = await _patientService.GetAllPatientsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Count(), patients.Count);
        _mockPatientRepository.Verify(repo => repo.GetAllPatientsAsync(), Times.Once);
    }
    [Fact]
    public async Task GetPatientByIdAsync_ShouldReturnMappedPatientDto_WhenPatientExists()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var patient = new Patient
        {
            PatientId = patientId,
            Name = "Jane Doe",
            Email = "jane@example.com",
            DateOfBirth = default,
            Gender = null
        };

        _mockPatientRepository.Setup(repo => repo.GetPatientByIdAsync(patientId))
            .ReturnsAsync(patient);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result.Name, patient.Name);
        Assert.Equal(result.Email, patient.Email);
        _mockPatientRepository.Verify(repo => repo.GetPatientByIdAsync(patientId), Times.Once);
    }

    [Fact]
    public async Task GetPatientByIdAsync_ShouldReturnNull_WhenPatientDoesNotExist()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        _mockPatientRepository.Setup(repo => repo.GetPatientByIdAsync(patientId))
            .ReturnsAsync((Patient)null);

        // Act
        var result = await _patientService.GetPatientByIdAsync(patientId);

        // Assert
        Assert.Null(result);
        _mockPatientRepository.Verify(repo => repo.GetPatientByIdAsync(patientId), Times.Once);
    }
    
    [Fact]
    public async Task AddPatientAsync_ShouldMapAndAddPatient()
    {
        // Arrange
        var createPatientDto = new CreatePatientDto
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            SSN = "123-45-6789",
            DateOfBirth = new DateTime(1980, 5, 15),
            Gender = "Female"
        };

        var expectedPatient = new Patient
        {
            Name = createPatientDto.Name,
            Email = createPatientDto.Email,
            SSN = createPatientDto.SSN,
            DateOfBirth = createPatientDto.DateOfBirth,
            Gender = createPatientDto.Gender
        };

        // Act
        await _patientService.AddPatientAsync(createPatientDto);

        // Assert
        _mockPatientRepository.Verify(repo => repo.AddPatientAsync(It.Is<Patient>(p =>
            p.Name == expectedPatient.Name &&
            p.Email == expectedPatient.Email &&
            p.SSN == expectedPatient.SSN &&
            p.DateOfBirth == expectedPatient.DateOfBirth &&
            p.Gender == expectedPatient.Gender
        )), Times.Once);
    }
    
    [Fact]
    public async Task UpdatePatientAsync_ShouldUpdatePatient_WhenPatientExists()
    {
        // Arrange
        var patientId = Guid.NewGuid();
        var updatePatientDto = new UpdatePatientDto
        {
            Name = "John Doe",
            Email = "updated@example.com",
        };

        var existingPatient = new Patient
        {
            PatientId = patientId,
            Name = "Jane Doe",
            Email = "original@example.com",
            Gender = "Female",
            DateOfBirth = default
        };

        _mockPatientRepository.Setup(repo => repo.GetPatientByIdAsync(patientId))
            .ReturnsAsync(existingPatient);

        // Act
        await _patientService.UpdatePatientAsync(patientId, updatePatientDto);

        // Assert
        _mockPatientRepository.Verify(repo => repo.UpdatePatientAsync(existingPatient), Times.Once);
    }

    [Fact]
    public async Task DeletePatientAsync_ShouldCallRepositoryDelete()
    {
        // Arrange
        var patientId = Guid.NewGuid();

        // Act
        await _patientService.DeletePatientAsync(patientId);

        // Assert
        _mockPatientRepository.Verify(repo => repo.DeletePatientAsync(patientId), Times.Once);
    }


}