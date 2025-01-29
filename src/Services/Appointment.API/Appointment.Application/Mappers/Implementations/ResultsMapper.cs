using Appointment.Application.Mappers.Interfaces;
using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Appointment.Application.Mappers.Implementations;

[Mapper]
public partial class ResultsMapper : IResultsMapper
{
	[MapperIgnoreTarget(nameof(Result.Appointment))]
	[MapperIgnoreTarget(nameof(Result.Id))]
	public partial Result ToModel(ResultRequestDto resultRequestDto);
	[MapperIgnoreSource(nameof(Result.Appointment))]
	public partial ResultResponseDto ToResponse(Result result);
	[MapperIgnoreSource(nameof(Result.Appointment))]
	public partial IEnumerable<ResultResponseDto> ToResponse(IEnumerable<Result> results);
}
