using Appointment.Application.Mappers.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Appointment.Application.Mappers.Implementations;

[Mapper(AllowNullPropertyAssignment = true)]
public partial class ResultsMapper : IResultsMapper
{
	[MapperIgnoreTarget(nameof(Result.Appointment))]
	[MapperIgnoreTarget(nameof(Result.Id))]
	public partial Result ToModel(ResultRequestDto resultRequestDto);
	public ResultResponseDto? ToResponse(Result? result)
	{
		if (result == null)
		{
			return null;
		}

		return new ResultResponseDto(
			result.Id,
			result.Complaints,
			result.Conclusion,
			result.Reccomendations,
			result.AppointmentId);
	}
	public IEnumerable<ResultResponseDto?> ToResponse(IEnumerable<Result?> results) => results.Select(ToResponse);
	[MapperIgnoreSource(nameof(Result.Appointment))]
	[MapperIgnoreSource(nameof(Result.AppointmentId))]
	[MapperIgnoreSource(nameof(Result.Id))]
	public partial ResultForDownloadResponseDto ToResponseForDownload(Result result);
	[MapperIgnoreTarget(nameof(Result.Appointment))]
	[MapperIgnoreTarget(nameof(Result.Id))]
	public partial void Update(ResultRequestDto destination, Result source);
}
