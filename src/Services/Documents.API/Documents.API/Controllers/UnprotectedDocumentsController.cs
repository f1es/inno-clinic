using Documents.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Documents.API.Controllers;

[ApiController]
[Route("api/unprotected/documents")]
public class UnprotectedDocumentsController : ControllerBase
{
	private readonly IDocumentService _documentService;

	public UnprotectedDocumentsController(IDocumentService documentService)
	{
		_documentService = documentService;
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
}
