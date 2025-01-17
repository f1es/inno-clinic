using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offices.Application.CQRS.Offices.Commands.CreateOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffices;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var getOfficesQuery = new GetOfficesQuery();

        var offices = await _mediator.Send(getOfficesQuery);

        return Ok(offices);
    }

    [HttpGet(Name = "GetOffice")]
    [Route("{id:guid}")]
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
}
