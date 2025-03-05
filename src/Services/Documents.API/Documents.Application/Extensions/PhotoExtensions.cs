using Documents.Core.Models;

namespace Documents.Application.Extensions;

public static class PhotoExtensions
{
	public static string GetFilename(this Photo photo) =>
		photo.Url.Split('/').Last();
}
