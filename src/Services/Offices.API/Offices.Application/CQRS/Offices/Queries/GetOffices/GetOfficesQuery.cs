using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Queries.GetOffices;

public class GetOfficesQuery : IRequest<IEnumerable<OfficeResponseDto>>
{

}
