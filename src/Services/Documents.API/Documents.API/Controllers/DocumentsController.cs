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

	[HttpGet]
	public async Task<IActionResult> GetAll()
	{
		var documents = await _documentService.GetAllAsync();

		return Ok(documents);
	}

	[HttpGet("{id:guid}", Name = "GetDocument")]
	public async Task<IActionResult> Get(Guid id)
	{
		var document = await _documentService.GetByIdAsync(id);

		return Ok(document);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Guid resultId, IFormFile documentFile)
	{
		var document = await _documentService.CreateAsync(resultId, documentFile);

		return CreatedAtRoute("GetDocument", new { id = document.Id }, document);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		await _documentService.DeleteAsync(id);

		return NoContent();
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, Guid resultId, IFormFile documentFile)
	{
		await _documentService.UpdateAsync(id, resultId, documentFile);

		return NoContent();
	}
}
