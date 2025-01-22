using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Application.CQRS.Services.Commands.CreateService;
using Services.Application.CQRS.Services.Commands.DeleteService;
using Services.Application.CQRS.Services.Commands.UpdateService;
using Services.Application.CQRS.Services.Queries.GetServiceById;
using Services.Application.CQRS.Services.Queries.GetServices;
using Services.Core.Dto.Request;

namespace Services.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServicesController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServicesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("{id:guid}", Name = "GetService")]
	public async Task<IActionResult> Get(Guid id)
	{
		var getServiceByIdQuery = new GetServiceByIdQuery(id);

		var service = await _mediator.Send(getServiceByIdQuery);

		return Ok(service);
	}

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var getServicesQuery = new GetServicesQuery();

		var services = await _mediator.Send(getServicesQuery);

		return Ok(services);
	}

	[HttpPost]
	public async Task<IActionResult> Create(CreateServiceCommand createServiceCommand)
	{
		var service = await _mediator.Send(createServiceCommand);

		return CreatedAtRoute("GetService", new { id = service.Id }, service);
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, ServiceRequestDto serviceRequestDto)
	{
		var updateServiceCommand = new UpdateServiceCommand(id, serviceRequestDto);

		await _mediator.Send(updateServiceCommand);

		return NoContent();
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		var deleteServiceCommand = new DeleteServiceCommand(id);

		await _mediator.Send(deleteServiceCommand);

		return NoContent();
	}
}
