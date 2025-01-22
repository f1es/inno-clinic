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

/// <summary>
/// Offices controller
/// </summary>
[ApiController]
[Route("api/offices")]
public class OfficesController : ControllerBase
{
	private readonly IMediator _mediator;

    public OfficesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all offices method
    /// </summary>
    /// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
    {
        var getOfficesQuery = new GetOfficesQuery();

        var offices = await _mediator.Send(getOfficesQuery);

        return Ok(offices);
    }

	/// <summary>
	/// Get office by id method
	/// </summary>
	/// <param name="id">Office's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetOffice")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
    {
        var getOfficeQuery = new GetOfficeQuery(id);

        var office = await _mediator.Send(getOfficeQuery);

        return Ok(office);
    }

    /// <summary>
    /// Create office method
    /// </summary>
    /// <param name="officeRequestDto">Request data transfer object of office</param>
    /// <returns></returns>
    [HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(CreateOfficeCommand createOfficeCommand)
    {
        var office = await _mediator.Send(createOfficeCommand);

		return CreatedAtRoute("GetOffice", new { id = office.Id }, office);  
    }

	/// <summary>
	/// Delete office method
	/// </summary>
	/// <param name="id">Office's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
    {
        var deleteOfficeCommand = new DeleteOfficeCommand(id);

        await _mediator.Send(deleteOfficeCommand);

        return NoContent();
    }

	/// <summary>
	/// Update office method
	/// </summary>
	/// <param name="id">Office's unique identifier</param>
	/// <param name="officeRequestDto">Request data transfer object of office</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, OfficeRequestDto officeRequestDto)
    {
        var updateOfficeCommand = new UpdateOfficeCommand(id, officeRequestDto);

        await _mediator.Send(updateOfficeCommand);

        return NoContent();
    }

}
