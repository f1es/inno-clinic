using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;
using Appointment.IntegrationTests.Utility;
using AutoFixture;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Appointment.IntegrationTests.Tests.TestServer;

public class ResultsControllerServerTests : TestServerBase
{
	private readonly Fixture _fixture;

    private const string ResultsEndpoint = "api/results/";
	private const string DownloadResultsEndpoint = "api/results/{id}/download";

	public ResultsControllerServerTests(WebAppFactory testsWebAppFactory) : base(testsWebAppFactory)
    {
		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => new TimeOnly(1, 1, 1));
	}

	[Fact]
	public async Task GET_BaseEndpoint_StatusCodeOk()
	{
		// Arrange
		var result = await AddResultToDbAsync();

		// Act
		var response = await _httpClient.GetAsync(ResultsEndpoint);

		// Assert
		var results = await response.Content.ReadFromJsonAsync<ResultResponseDto[]>();
		results.First().Should().BeEquivalentTo(result, options => options.Excluding(x => x.Appointment));
		Assert.Equivalent(200, (int)response.StatusCode);
	}


	[Fact]
	public async Task GET_BaseEndpointWithId_ValidIdFromQuery_StatusCodeOk()
	{
		// Arrange
		var result = await AddResultToDbAsync();
		var id = result.Id;
		var url = ResultsEndpoint + id.ToString();

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		var resultResponse = await response.Content.ReadFromJsonAsync<ResultResponseDto>();
		resultResponse.Should().BeEquivalentTo(result, options => options.Excluding(x => x.Appointment));

		Assert.Equivalent(200, (int)response.StatusCode);
	}

	[Fact]
	public async Task GET_BaseEndpointWithId_InvalidIdFromQuery_StatusCodeNotFound()
	{
		// Arrange
		var id = new Guid();
		var url = ResultsEndpoint + id.ToString();

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	[Fact]
	public async Task GET_DownloadEndpointWithId_ValidIdFromQuery_StatusCodeOk()
	{
		// Arrange
		var result = await AddResultToDbAsync();
		var id = result.Id;
		var url = DownloadResultsEndpoint.Replace("{id}", id.ToString());

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		Assert.Equivalent(200, (int)response.StatusCode);
	}

	[Fact]
	public async Task GET_DownloadEndpointWithId_InvalidIdFromQuery_StatusNotFound()
	{
		// Arrange
		var url = DownloadResultsEndpoint.Replace("{id}", new Guid().ToString());

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	[Fact]
	public async Task POST_BaseEndpoint_ValidObjectRequestFromBody_StatusCodeCreated()
	{
		// Arrange
		var appointment = await AddAppointmentToDbAsync();
		var resultRequest = _fixture.Create<ResultRequestDto>();
		resultRequest = resultRequest with { AppointmentId = appointment.Id };
		var httpContent = GetHttpContentOfRequest(resultRequest);

		// Act
		var response = await _httpClient.PostAsync(ResultsEndpoint, httpContent);

		// Assert
		Assert.Equivalent(201, (int)response.StatusCode);
	}

	[Fact]
	public async Task PUT_BaseEndpointWithId_ValidObjectRequestFromBodyAndValidIdFromQuery_StatusCodeNoContent()
	{
		// Arrange
		var result = await AddResultToDbAsync();
		var resultRequest = _fixture.Create<ResultRequestDto>();
		resultRequest = resultRequest with { AppointmentId = result.AppointmentId };
		var httpContent = GetHttpContentOfRequest(resultRequest);
		var url = ResultsEndpoint + result.Id.ToString();

		// Act 
		var response = await _httpClient.PutAsync(url, httpContent);

		// Assert
		Assert.Equivalent(204, (int)response.StatusCode);
		var updatedResult = await _context.Results.FindAsync(result.Id);
		updatedResult.Should().BeEquivalentTo(resultRequest);
	}

	[Fact]
	public async Task PUT_BaseEndpointWithId_ValidObjectRequestFromBodyAndInvalidIdFromQuery_StatusCodeNotFound()
	{
		// Arrange
		var resultRequest = _fixture.Create<ResultRequestDto>();
		var httpContent = GetHttpContentOfRequest(resultRequest);
		var url = ResultsEndpoint + new Guid();

		// Act 
		var response = await _httpClient.PutAsync(url, httpContent);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	[Fact]
	public async Task DELETE_BaseEndpointWithId_ValidIdFromQuery_StatusCodeNoContent()
	{
		// Arrange
		var result = await AddResultToDbAsync();
		var id = result.Id;
		var url = ResultsEndpoint + id.ToString();

		// Act
		var response = await _httpClient.DeleteAsync(url);

		// Assert
		Assert.Equivalent(204, (int)response.StatusCode);
	}

	[Fact]
	public async Task DELETE_BaseEndpointWithId_InvalidIdFromQuery_StatusCodeNotFound()
	{
		// Arrange
		var appointment = await AddResultToDbAsync();
		var url = ResultsEndpoint + new Guid().ToString();

		// Act
		var response = await _httpClient.DeleteAsync(url);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	private async Task<Result> AddResultToDbAsync()
	{
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		_context.Appointments.Add(appointment);
		var result = _fixture.Build<Result>().Without(x => x.Appointment).Create();
		result.AppointmentId = appointment.Id;
		_context.Results.Add(result);
		await _context.SaveChangesAsync(default);
		_context.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
		return result;
	}

	private async Task<Core.Models.Appointment> AddAppointmentToDbAsync()
	{
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
		_context.Appointments.Add(appointment);
		await _context.SaveChangesAsync(default);
		_context.Entry(appointment).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
		return appointment;
	}

	private HttpContent GetHttpContentOfRequest(ResultRequestDto resultRequest) =>
		new StringContent(JsonSerializer.Serialize(resultRequest), Encoding.UTF8, "application/json");
}
