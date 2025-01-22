using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Queries.GetOffice;

public record GetOfficeQuery(Guid Id) : IRequest<OfficeResponseDto>
{ }
