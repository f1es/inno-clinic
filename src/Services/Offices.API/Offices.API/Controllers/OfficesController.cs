using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offices.Application.CQRS.Offices.Commands.CreateOffice;
using Offices.Application.CQRS.Offices.Commands.DeleteOffice;
using Offices.Application.CQRS.Offices.Commands.UpdateOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffices;
using Offices.Application.CQRS.Receptionists.Queries.GetByOfficeId;
using Offices.Core.Dto.Request;

namespace Offices.API.Controllers;

[ApiController]
[Route("api/offices")]
public class OfficesController : ControllerBase
{
	private readonly IMediator _mediator;

    public OfficesController(IMediator mediator)
    {
        _mediator = mediator;
    }

	[HttpGet("{id:guid}/receptionist")]
	public async Task<IActionResult> GetReceptionist(Guid id)
	{
		var getReceptionistByOfficeIdQuery = new GetReceptionistByOfficeIdQuery(id);

        var receptionist = await _mediator.Send(getReceptionistByOfficeIdQuery);

		return Ok(receptionist);
	}

	[HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var getOfficesQuery = new GetOfficesQuery();

        var offices = await _mediator.Send(getOfficesQuery);

        return Ok(offices);
    }

    [HttpGet("{id:guid}", Name = "GetOffice")]
    public async Task<IActionResult> Get(Guid id)
    {
        var getOfficeQuery = new GetOfficeQuery(id);

        var office = await _mediator.Send(getOfficeQuery);

        return Ok(office);
    }

    [HttpPost]
    public async Task<IActionResult> Create(OfficeRequestDto officeRequestDto)
    {
        var createOfficeCommand = new CreateOfficeCommand(officeRequestDto);

        var office = await _mediator.Send(createOfficeCommand);

        var routeName = "GetOffice";

		return CreatedAtRoute(routeName, new { id = office.Id }, office);  
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteOfficeCommand = new DeleteOfficeCommand(id);

        await _mediator.Send(deleteOfficeCommand);

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, OfficeRequestDto officeRequestDto)
    {
        var updateOfficeCommand = new UpdateOfficeCommand(id, officeRequestDto);

        await _mediator.Send(updateOfficeCommand);

        return NoContent();
    }

}
