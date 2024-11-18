using MeasurementService.DTOs;
using MeasurementService.Models;

namespace MeasurementService.Mapper;

using AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Measurement, MeasurementDto>();
        CreateMap<CreateMeasurementDto, Measurement>();

    }
}