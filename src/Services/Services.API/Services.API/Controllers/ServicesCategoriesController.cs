using MediatR;
using Microsoft.AspNetCore.Mvc;
using Services.Application.CQRS.ServicesCategories.Commands.CreateServiceCategory;
using Services.Application.CQRS.ServicesCategories.Commands.DeleteServiceCategory;
using Services.Application.CQRS.ServicesCategories.Commands.UpdateServiceCategory;
using Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategories;
using Services.Application.CQRS.ServicesCategories.Queries.GetServiceCategoryById;
using Services.Core.Dto.Request;

namespace Services.API.Controllers;

/// <summary>
/// Controller for working with services categories
/// </summary>
[ApiController]
[Route("api/services-categories")]
public class ServicesCategoriesController : ControllerBase
{
	private readonly IMediator _mediator;

	public ServicesCategoriesController(IMediator mediator)
	{
		_mediator = mediator;
	}

	/// <summary>
	/// Get service category by id method
	/// </summary>
	/// <param name="id">service category's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetServiceCategory")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
	{
		var getServiceCategoryByIdQuery = new GetServiceCategoryByIdQuery(id);

		var serviceCategory = await _mediator.Send(getServiceCategoryByIdQuery, cancellationToken);

		return Ok(serviceCategory);
	}

	/// <summary>
	/// Get service categories method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
	{
		var getServiceCategoriesQuery = new GetServiceCategoriesQuery();

		var serviceCategories = await _mediator.Send(getServiceCategoriesQuery, cancellationToken);

		return Ok(serviceCategories);
	}

	/// <summary>
	/// Create service category method
	/// </summary>
	/// <param name="serviceCategoryRequestDto">Request data tranfer object of service category</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(ServiceCategoryRequestDto serviceCategoryRequestDto, CancellationToken cancellationToken)
	{
		var createServiceCategoryCommand = new CreateServiceCategoryCommand(serviceCategoryRequestDto);

		var serviceCategory = await _mediator.Send(createServiceCategoryCommand, cancellationToken);

		return CreatedAtRoute("GetServiceCategory", new { id = serviceCategory.Id }, serviceCategory);
	}

	/// <summary>
	/// Delete service category method
	/// </summary>
	/// <param name="id">Service category's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
	{
		var deleteServiceCategoryCommand = new DeleteServiceCategoryCommand(id);

		await _mediator.Send(deleteServiceCategoryCommand, cancellationToken);

		return NoContent();
	}

	/// <summary>
	/// Update service category method
	/// </summary>
	/// <param name="id">Service category's unique identifier</param>
	/// <param name="serviceCategoryRequestDto">Request data tranfer object of service category</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, ServiceCategoryRequestDto serviceCategoryRequestDto, CancellationToken cancellationToken)
	{
		var updateServiceCategoryCommand = new UpdateServiceCategoryCommand(id, serviceCategoryRequestDto);

		await _mediator.Send(updateServiceCategoryCommand, cancellationToken);

		return NoContent();
	}
}
