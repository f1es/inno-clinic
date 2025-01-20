using MediatR;
using Offices.Core.Dto.Response;

namespace Offices.Application.CQRS.Receptionists.Queries.GetReceptionist;

public class GetReceptionistQuery : IRequest<ReceptionistResponseDto>
{
    public Guid Id { get; set; }
    public GetReceptionistQuery(Guid id)
    {
        Id = id;
    }
}
