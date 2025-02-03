namespace Profiles.Core.Parameters;

public class RequestParameters
{
	public int Page { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string SearchTerm { get; set; } = string.Empty;
	public string OrderQuery { get; set; } = string.Empty;
}
