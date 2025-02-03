using Microsoft.AspNetCore.Http;

namespace Documents.Core.Dtos.Request;

public record DocumentRequestDto(IFormFile Document);
