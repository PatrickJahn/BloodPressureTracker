using AutoMapper;
using PatientService.Models;
using PatientService.DTOs;

namespace PatientService.Mappers
{
    public class PatientMappingProfile : Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<Patient, PatientDto>().ReverseMap();

            CreateMap<CreatePatientDto, Patient>();

            CreateMap<UpdatePatientDto, Patient>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}