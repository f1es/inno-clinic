using Documents.Core.Models;

namespace Documents.Application.Extensions;

public static class DocumentsExtensions
{
	public static string GetFilename(this Document document) =>
		document.Url.Split('/').Last();
}
