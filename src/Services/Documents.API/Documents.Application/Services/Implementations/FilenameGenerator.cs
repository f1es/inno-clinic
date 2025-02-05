using Documents.Application.Services.Interfaces;

namespace Documents.Application.Services.Implementations;

public class FilenameGenerator : IFilenameGenerator
{
	public string Generate(string filename)
	{
		var fileType = filename.Split('.').Last();

		return $"{DateTime.UtcNow.ToString("MM-dd-yyyy-HH-mm-ss")}-{Guid.NewGuid()}.{fileType}";
	}	
}
