using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Queries.GetReceptionists;

public record GetReceptionistsQuery : IRequest<IEnumerable<ReceptionistResponseDto>>
{ }
