using Documents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Documents.API.Controllers;

[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
	private readonly IDocumentService _documentService;

	public DocumentsController(IDocumentService documentService)
	{
		_documentService = documentService;
	}

	/// <summary>
	/// Get all documents method
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> GetAll()
	{
		var documents = await _documentService.GetAllAsync();

		return Ok(documents);
	}

	/// <summary>
	///  Get document by id method
	/// </summary>
	/// <param name="id">Document's unique identifier</param>
	/// <returns></returns>
	[HttpGet("{id:guid}", Name = "GetDocument")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Get(Guid id)
	{
		var document = await _documentService.GetByIdAsync(id);

		return Ok(document);
	}

	/// <summary>
	/// Create document method
	/// </summary>
	/// <param name="resultId">Foreign key of reuslt</param>
	/// <param name="documentFile">Document file</param>
	/// <returns></returns>
	[HttpPost]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Create(Guid resultId, IFormFile documentFile)
	{
		var document = await _documentService.CreateAsync(resultId, documentFile);

		return CreatedAtRoute("GetDocument", new { id = document.Id }, document);
	}

	/// <summary>
	/// Delete document method
	/// </summary>
	/// <param name="id">Document's unique identifier</param>
	/// <returns></returns>
	[HttpDelete("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _documentService.DeleteAsync(id);

		return NoContent();
	}

	/// <summary>
	/// Update document method
	/// </summary>
	/// <param name="id">Document's unique identifier</param>
	/// <param name="resultId">Foreign key of reuslt</param>
	/// <param name="documentFile">Document file</param>
	/// <returns></returns>
	[HttpPut("{id:guid}")]
	[Produces("application/json")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> Update(Guid id, Guid resultId, IFormFile documentFile)
	{
		await _documentService.UpdateAsync(id, resultId, documentFile);

		return NoContent();
	}
}
