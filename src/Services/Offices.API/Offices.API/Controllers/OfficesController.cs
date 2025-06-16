using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Offices.Application.CQRS.Offices.Commands.CreateOffice;
using Offices.Application.CQRS.Offices.Commands.DeleteOffice;
using Offices.Application.CQRS.Offices.Commands.UpdateOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffice;
using Offices.Application.CQRS.Offices.Queries.GetOffices;
using Offices.Core.Dto.Request;

namespace Offices.API.Controllers;

/// <summary>
/// Offices controller
/// </summary>
[ApiController]
[Route("api/offices")]
//[Authorize]
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
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var getOfficesQuery = new GetOfficesQuery();

        var offices = await _mediator.Send(getOfficesQuery, cancellationToken);

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
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        var getOfficeQuery = new GetOfficeQuery(id);

        var office = await _mediator.Send(getOfficeQuery, cancellationToken);

        return Ok(office);
    }

    /// <summary>
    /// Create office method
    /// </summary>
    /// <param name="officeRequestDto">Request data transfer object of office</param>
    /// <returns></returns>
    [HttpPost]
	[Produces("application/json")]
	//[Authorize(Roles = "receptionist")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(CreateOfficeCommand createOfficeCommand, CancellationToken cancellationToken)
    {
        var office = await _mediator.Send(createOfficeCommand, cancellationToken);

		return CreatedAtRoute("GetOffice", new { id = office.Id }, office);  
    }

	/// <summary>
	/// Delete office method
	/// </summary>
	/// <param name="id">Office's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	//[Authorize(Roles = "receptionist")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleteOfficeCommand = new DeleteOfficeCommand(id);

        await _mediator.Send(deleteOfficeCommand, cancellationToken);

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
	//[Authorize(Roles = "receptionist")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, OfficeRequestDto officeRequestDto, CancellationToken cancellationToken)
    {
        var updateOfficeCommand = new UpdateOfficeCommand(id, officeRequestDto);

        await _mediator.Send(updateOfficeCommand, cancellationToken);

        return NoContent();
    }

}
