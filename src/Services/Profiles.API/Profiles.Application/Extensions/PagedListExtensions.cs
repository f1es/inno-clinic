using Mapster;
using Profiles.Core.Utility;

namespace Profiles.Application.Extensions;

public static class PagedListExtensions
{
	public static PagedList<TNew> AdaptPagedList<TOld, TNew>(this PagedList<TOld> pagedList)
	{
		var newValues = pagedList.Values.Adapt<IEnumerable<TNew>>();

		var newPagedList = new PagedList<TNew>(newValues, pagedList.PageData);

		return newPagedList;
	}
}
