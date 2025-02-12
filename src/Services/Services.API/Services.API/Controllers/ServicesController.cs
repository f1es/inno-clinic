using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Application.CQRS.Services.Commands.CreateService;
using Services.Application.CQRS.Services.Commands.DeleteService;
using Services.Application.CQRS.Services.Commands.UpdateService;
using Services.Application.CQRS.Services.Queries.GetServiceById;
using Services.Application.CQRS.Services.Queries.GetServices;
using Services.Application.Dto.Request;

namespace Services.API.Controllers;

/// <summary>
/// Controller for working with services
/// </summary>
[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServicesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Get service by id method
	/// </summary>
	/// <param name="id">Service's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetService")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var getServiceByIdQuery = new GetServiceByIdQuery(id);

		var service = await _mediator.Send(getServiceByIdQuery);

		return Ok(service);
	}

	/// <summary>
	/// Get services method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var getServicesQuery = new GetServicesQuery();

		var services = await _mediator.Send(getServicesQuery);

		return Ok(services);
	}

	/// <summary>
	/// Create service method
	/// </summary>
	/// <param name="serviceRequestDto">Request service data transfer object</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(ServiceRequestDto serviceRequestDto)
	{
		var createServiceCommand = new CreateServiceCommand(serviceRequestDto);

		var service = await _mediator.Send(createServiceCommand);

		return CreatedAtRoute("GetService", new { id = service.Id }, service);
	}

	/// <summary>
	/// Update service method
	/// </summary>
	/// <param name="id">Service's unique identifier</param>
	/// <param name="serviceRequestDto">Request service data transfer object</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, ServiceRequestDto serviceRequestDto)
	{
		var updateServiceCommand = new UpdateServiceCommand(id, serviceRequestDto);

		await _mediator.Send(updateServiceCommand);

		return NoContent();
	}

	/// <summary>
	/// Delete service method
	/// </summary>
	/// <param name="id">Service's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		var deleteServiceCommand = new DeleteServiceCommand(id);

		await _mediator.Send(deleteServiceCommand);

		return NoContent();
	}
}
