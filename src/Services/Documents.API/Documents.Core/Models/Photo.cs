namespace Documents.Core.Models;

public class Photo
{
	public Guid Id { get; set; }
	public string Url { get; set; }

    public Photo(string url)
    {
        Url = url;
    }
}
