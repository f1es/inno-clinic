namespace Profiles.Application.Utility;

public class PagedList<T>
{
	public List<T> Values { get; set; }
	public int MaxPage { get; set; }
	public int Page { get; set; }
	public bool HasNext { get; set; }
	public bool HasPrevious { get; set; }

    public PagedList(
		List<T> values,
		int maxPage,
		int page,
		bool hasNext,
		bool hasPrevious)
    {
        Values = values;
		MaxPage = maxPage;
		Page = page;
		HasNext = hasNext;
		HasPrevious = hasPrevious;
    }
}
