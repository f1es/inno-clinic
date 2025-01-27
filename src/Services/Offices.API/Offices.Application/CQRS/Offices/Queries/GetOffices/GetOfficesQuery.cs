using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Queries.GetOffices;

public record GetOfficesQuery() : IRequest<IEnumerable<OfficeResponseDto>> 
{ }