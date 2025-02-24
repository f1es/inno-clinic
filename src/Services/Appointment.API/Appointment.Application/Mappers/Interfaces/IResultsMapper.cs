using Appointment.Core.Dto.Request;
using Appointment.Core.Dto.Response;
using Appointment.Core.Models;

namespace Appointment.Application.Mappers.Interfaces;

public interface IResultsMapper
{
    Result ToModel(ResultRequestDto resultRequestDto);
    IEnumerable<ResultResponseDto> ToResponse(IEnumerable<Result> results);
    ResultResponseDto ToResponse(Result result);
    ResultForDownloadResponseDto ToResponseForDownload(Result result);
	void Update(ResultRequestDto destination, Result source);
}