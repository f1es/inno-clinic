namespace Profiles.Core.Utility;

public class PagedList<T>
{
	public List<T> Values { get; }
	public PageData PageData { get; }

    public PagedList(
		IEnumerable<T> values,
		int maxPage,
		int page,
		bool hasNext,
		bool hasPrevious)
    {
        Values = values.ToList();
		PageData = new PageData(maxPage, page, hasNext, hasPrevious);
    }

    public PagedList(
		IEnumerable<T> values, 
		PageData pageData)
    {
        Values= values.ToList();
		PageData = pageData;
    }
}
