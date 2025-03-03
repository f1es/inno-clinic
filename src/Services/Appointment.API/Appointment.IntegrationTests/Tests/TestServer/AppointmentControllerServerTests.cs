using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.IntegrationTests.Utility;
using AutoFixture;
using FluentAssertions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Appointment.IntegrationTests.Tests.TestServer;

public class AppointmentControllerServerTests : TestServerBase
{
	private readonly Fixture _fixture;

    private const string AppointmentsEndpoint = "/api/appointments/";

	public AppointmentControllerServerTests(WebAppFactory testsWebAppFactory) : base(testsWebAppFactory)
    {
		_fixture = new Fixture();
		_fixture.Register(() => DateOnly.FromDateTime(_fixture.Create<DateTime>()));
		_fixture.Register(() => new TimeOnly(1, 1, 1));
	}

	public static IEnumerable<object[]> InvalidGuidTestData()
	{
		yield return new object[] { new Guid() };
		yield return new object[] { Guid.Parse("11111111-1111-1111-1111-111111111111") };
	}

    [Fact]
    public async Task GET_Endpoint_StatusCodeOk()
    {
		// Arrange
		var appointment = await AddAppointmentToDbAsync();

        // Act
        var response = await _httpClient.GetAsync(AppointmentsEndpoint);

        // Assert
        var appointments = await response.Content.ReadFromJsonAsync<AppointmentResponseDto[]>(); 
		appointments.First().Should().BeEquivalentTo(appointment, options => options.Excluding(x => x.Result));

        Assert.Single(appointments);
        Assert.Equivalent(200, (int)response.StatusCode);
    }

    [Fact]
    public async Task GET_EndpointWithId_ValidIdFromQuery_StatusCodeOk()
    {
		// Arrange
		var appointment = await AddAppointmentToDbAsync();
        var id = appointment.Id;
        var url = AppointmentsEndpoint + id.ToString();

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		var appointmentResponse = await response.Content.ReadFromJsonAsync<AppointmentResponseDto>();
        appointmentResponse.Should().BeEquivalentTo(appointment, options => options.Excluding(x => x.Result));

        Assert.Equivalent(200, (int)response.StatusCode);
	}

    [Theory]
	[MemberData(nameof(InvalidGuidTestData))]
	public async Task GET_EndpointWithId_InvalidIdFromQuery_StatusCodeNotFound(Guid invalidId)
	{
		// Arrange
		//var id = new Guid();
        var url = AppointmentsEndpoint + invalidId.ToString();

		// Act
		var response = await _httpClient.GetAsync(url);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	[Fact]
    public async Task POST_Endpoint_ValidObjectRequestFromBody_StatusCodeCreated()
    {
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
        var httpContent = GetHttpContentOfRequest(appointmentRequest);

        // Act
        var response = await _httpClient.PostAsync(AppointmentsEndpoint, httpContent);

		// Assert
		Assert.Equivalent(201, (int)response.StatusCode);
    }

    [Fact]
	public async Task PUT_EndpointWithId_ValidObjectRequestFromBodyAndValidIdFromQuery_StatusCodeNoContent()
    {
        // Arrange
        var appointment = await AddAppointmentToDbAsync();
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
        var httpContent = GetHttpContentOfRequest(appointmentRequest);
        var url = AppointmentsEndpoint + appointment.Id.ToString();

		// Act 
		var response = await _httpClient.PutAsync(url, httpContent);

		// Assert
        Assert.Equivalent(204, (int)response.StatusCode);
        var updatedAppointment = await _context.Appointments.FindAsync(appointment.Id);
        updatedAppointment.Should().BeEquivalentTo(appointmentRequest);
	}

	[Theory]
	[MemberData(nameof(InvalidGuidTestData))]
	public async Task PUT_EndpointWithId_ValidObjectRequestFromBodyAndInvalidIdFromQuery_StatusCodeNotFound(Guid invalidId)
	{
		// Arrange
		var appointmentRequest = _fixture.Create<AppointmentRequestDto>();
		var httpContent = GetHttpContentOfRequest(appointmentRequest);
		var url = AppointmentsEndpoint + invalidId.ToString();

		// Act 
		var response = await _httpClient.PutAsync(url, httpContent);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	[Fact]
	public async Task DELETE_EndpointWithId_ValidIdFromQuery_StatusCodeNoContent()
	{
		// Arrange
		var appointment = await AddAppointmentToDbAsync();
		var id = appointment.Id;
		var url = AppointmentsEndpoint + id.ToString();

		// Act
		var response = await _httpClient.DeleteAsync(url);

		// Assert
		Assert.Equivalent(204, (int)response.StatusCode);
	}

	[Theory]
	[MemberData(nameof(InvalidGuidTestData))]
	public async Task DELETE_EndpointWithId_InvalidIdFromQuery_StatusCodeNotFound(Guid invalidId)
	{
		// Arrange
		var appointment = await AddAppointmentToDbAsync();
		var url = AppointmentsEndpoint + invalidId.ToString();

		// Act
		var response = await _httpClient.DeleteAsync(url);

		// Assert
		Assert.Equivalent(404, (int)response.StatusCode);
	}

	private async Task<Core.Models.Appointment> AddAppointmentToDbAsync()
    {
		var appointment = _fixture.Build<Core.Models.Appointment>().Without(x => x.Result).Create();
        _context.Appointments.Add(appointment);
		await _context.SaveChangesAsync(default);
        _context.Entry(appointment).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        return appointment;
	}

    private HttpContent GetHttpContentOfRequest(AppointmentRequestDto appointmentRequest) =>
		new StringContent(JsonSerializer.Serialize(appointmentRequest), Encoding.UTF8, "application/json");
}
