using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Queries.GetReceptionist;

public record GetReceptionistQuery(Guid Id) : IRequest<ReceptionistResponseDto>
{ }
