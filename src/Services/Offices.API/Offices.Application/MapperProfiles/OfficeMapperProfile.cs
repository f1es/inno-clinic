using AutoMapper;
using Offices.Core.Dto.Request;
using Offices.Core.Dto.Response;
using Offices.Core.Models;

namespace Offices.Application.MapperProfiles;

public class OfficeMapperProfile : Profile
{
    public OfficeMapperProfile()
    {
        CreateMap<OfficeRequestDto, Office>();

        CreateMap<Office, OfficeResponseDto>();
    }
}
