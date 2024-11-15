using MeasurementService.DTOs;
using MeasurementService.Models;

namespace MeasurementService.Mapper;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mapping configurations here
        CreateMap<Measurement, MeasurementDto>();
        CreateMap<CreateMeasurementDto, Measurement>();

    }
}