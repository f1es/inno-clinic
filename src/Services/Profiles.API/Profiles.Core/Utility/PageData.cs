namespace Profiles.Core.Utility;

public class PageData
{
	public int MaxPage { get; }
	public int Page { get; }
	public bool HasNext { get; }
	public bool HasPrevious { get; }

	public PageData(
		int maxPage,
		int page, 
		bool hasNext,
		bool hasPrevious)
	{
		MaxPage = maxPage;
		Page = page;
		HasNext = hasNext;
		HasPrevious = hasPrevious;
	}
}
