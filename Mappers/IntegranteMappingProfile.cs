using AutoMapper;
using EscalaApi.Data.DTOs;
using EscalaApi.Data.Entities;

namespace EscalaApi.Mappers;

public class IntegranteMappingProfile : Profile
{
    public IntegranteMappingProfile()
    {
        CreateMap<IntegranteDto, Integrante>()
            .ForMember(opt => opt.Nome, opt => opt.MapFrom(src => src.Nome));
    }
}