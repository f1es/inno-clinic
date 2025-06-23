using Appointment.Core.RequestClients;
using Appointment.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text;
using System.Text.Json;
using Shared.Options;

namespace Appointment.Infrastructure.RequestClients;

public class DocumentsRequestClient : IDocumentRequestClient
{
	private readonly HttpClient _httpClient;
	private readonly DocumentsEndpointsOptions _documentsEndpoints;

	public DocumentsRequestClient(HttpClient httpClient, IOptions<DocumentsEndpointsOptions> documentsEndpoints)
	{
		_httpClient = httpClient;
		_documentsEndpoints = documentsEndpoints.Value;
	}

	public async Task CreateDocumentAsync(MemoryStream documentStream)
	{
		using var streamContent = new StreamContent(documentStream);
		using var content = new MultipartFormDataContent();
		streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
		var name = "documentFile";
		var fileName = "documentFile.pdf";
		content.Add(streamContent, name, fileName);

		var response = await _httpClient.PostAsync($"{_documentsEndpoints.Url}{_documentsEndpoints.DocumentsEndpoint}", content);
		await HandleResponseAsync(response);
	}

	private async Task<HttpResponseMessage> HandleResponseAsync(HttpResponseMessage response)
	{
		if (!response.IsSuccessStatusCode)
		{
			var responseMessage = await response.Content.ReadAsStringAsync();
			var responseJson = JsonSerializer.Deserialize<JsonElement>(responseMessage);
			var errorMessage = responseJson.GetProperty("error");

			throw new Exception(errorMessage.ToString());
		}

		return response;
	}
}
