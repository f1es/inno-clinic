using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Offices.Queries.GetOffice;

public class GetOfficeQuery : IRequest<OfficeResponseDto>
{
	public Guid Id { get; set; }
    public GetOfficeQuery(Guid id)
    {
        Id = id;
    }
}
