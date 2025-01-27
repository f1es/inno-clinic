using AutoMapper;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;
using Offices.Core.Models;

namespace Offices.Application.MapperProfiles;

public class ReceptionistMapperProfile : Profile
{
    public ReceptionistMapperProfile()
    {
        CreateMap<ReceptionistRequestDto, Receptionist>();

        CreateMap<Receptionist, ReceptionistResponseDto>();
    }
}
