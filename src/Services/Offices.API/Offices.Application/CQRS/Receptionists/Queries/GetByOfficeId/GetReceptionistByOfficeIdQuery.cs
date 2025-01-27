using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Queries.GetByOfficeId;

public record GetReceptionistByOfficeIdQuery(Guid OfficeId) : IRequest<ReceptionistResponseDto>
{ }
