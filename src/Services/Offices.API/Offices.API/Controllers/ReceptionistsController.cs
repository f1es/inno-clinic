using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Offices.Application.CQRS.Receptionists.Commands.CreateRecepcionist;
using Offices.Application.CQRS.Receptionists.Commands.DeleteREceptionist;
using Offices.Application.CQRS.Receptionists.Commands.UpdateReceptionist;
using Offices.Application.CQRS.Receptionists.Queries.GetByOfficeId;
using Offices.Application.CQRS.Receptionists.Queries.GetReceptionist;
using Offices.Application.CQRS.Receptionists.Queries.GetReceptionists;
using Offices.Core.Dto.Request;

namespace Offices.API.Controllers;

/// <summary>
/// Receptionists controller
/// </summary>
[ApiController]
[Authorize]
[Route("api/receptionists")]
public class ReceptionistsController : ControllerBase
{
	private readonly IMediator _mediator;

	public ReceptionistsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Get all receptionists method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var getAllReceptionistsQuery = new GetReceptionistsQuery();

		var receptionists = await _mediator.Send(getAllReceptionistsQuery);

		return Ok(receptionists);
	}

	/// <summary>
	/// Get receptionist by id method
	/// </summary>
	/// <param name="id">Receptionist's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetReceptionistById")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var getReceptionistQuery = new GetReceptionistQuery(id);

		var receptionist = await _mediator.Send(getReceptionistQuery);

		return Ok(receptionist);
	}

	/// <summary>
	/// Get receptionist by office id method, office have only one receptionist
	/// </summary>
	/// <param name="officeId">Office's unique identifier</param>
	/// <returns></returns>
	[HttpGet("office/{officeId:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetByOfficeId(Guid officeId)
	{
		var getReceptionistByOfficeIdQuery = new GetReceptionistByOfficeIdQuery(officeId);

		var receptionist = await _mediator.Send(getReceptionistByOfficeIdQuery);

		return Ok(receptionist);
	}

	/// <summary>
	/// Create receptionist method
	/// </summary>
	/// <param name="receptionistRequestDto">Request receptionist data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status409Conflict)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(CreateReceptionistCommand createReceptionistDto)
	{
		var receptionist = await _mediator.Send(createReceptionistDto);

		return CreatedAtRoute("GetReceptionistById", new { id = receptionist.Id }, receptionist);
	}

	/// <summary>
	/// Delete receptionist method
	/// </summary>
	/// <param name="id">Recepcionist's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		var deleteReceptionistCommand = new DeleteReceptionistCommand(id);

		await _mediator.Send(deleteReceptionistCommand);

		return NoContent();
	}

	/// <summary>
	/// Update receptionist method
	/// </summary>
	/// <param name="id">Receptionist's unique identifier</param>
	/// <param name="receptionistRequestDto">Request receptionist data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, ReceptionistRequestDto receptionistRequestDto)
	{
		var updateReceptionistCommand = new UpdateReceptionistCommand(id, receptionistRequestDto);

		await _mediator.Send(updateReceptionistCommand);

		return NoContent();
	}
}
