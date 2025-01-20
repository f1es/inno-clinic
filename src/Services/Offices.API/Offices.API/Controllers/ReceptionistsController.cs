using MediatR;
using Microsoft.AspNetCore.Mvc;
using Offices.Application.CQRS.Receptionists.Commands.CreateRecepcionist;
using Offices.Application.CQRS.Receptionists.Commands.DeleteREceptionist;
using Offices.Application.CQRS.Receptionists.Commands.UpdateReceptionist;
using Offices.Application.CQRS.Receptionists.Queries.GetReceptionist;
using Offices.Application.CQRS.Receptionists.Queries.GetReceptionists;
using Offices.Core.Dto.Request;

namespace Offices.API.Controllers;

[ApiController]
[Route("api/receptionists")]
public class ReceptionistsController : ControllerBase
{
	private readonly IMediator _mediator;

	public ReceptionistsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var getAllReceptionistsQuery = new GetReceptionistsQuery();

		var receptionists = await _mediator.Send(getAllReceptionistsQuery);

		return Ok(receptionists);
	}

	[HttpGet("{id:guid}", Name = "GetReceptionistById")]
	public async Task<IActionResult> Get(Guid id)
	{
		var getReceptionistQuery = new GetReceptionistQuery(id);

		var receptionist = await _mediator.Send(getReceptionistQuery);

		return Ok(receptionist);
	}

	[HttpPost]
	public async Task<IActionResult> Create(ReceptionistRequestDto receptionistRequestDto)
	{
		var createReceptionistDto = new CreateReceptionistCommand(receptionistRequestDto);

		var receptionist = await _mediator.Send(createReceptionistDto);

		var routeName = "GetReceptionistById";

		return CreatedAtRoute(routeName , new { id = receptionist.Id }, receptionist);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var deleteReceptionistCommand = new DeleteReceptionistCommand(id);

		await _mediator.Send(deleteReceptionistCommand);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		var updateReceptionistCommand = new UpdateReceptionistCommand(id, receptionistRequestDto);

		await _mediator.Send(updateReceptionistCommand);

		return NoContent();
	}
}
