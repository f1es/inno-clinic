namespace Documents.Core.Models;

public class Document
{
	public Guid Id { get; set; }
	public string Url { get; set; }
	public Guid ResultId { get; set; }

    public Document(string url, Guid resultId)
    {
        Url = url;
        ResultId = resultId;
    }
}
