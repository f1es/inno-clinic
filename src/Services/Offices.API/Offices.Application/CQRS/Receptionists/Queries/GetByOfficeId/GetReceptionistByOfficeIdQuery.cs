using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Queries.GetByOfficeId;

public class GetReceptionistByOfficeIdQuery : IRequest<ReceptionistResponseDto>
{
	public Guid OfficeId { get; set; }

	public GetReceptionistByOfficeIdQuery(Guid officeId)
	{
		OfficeId = officeId;
	}
}
