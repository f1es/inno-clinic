using Profiles.Application.Utility;
using Shared.Exceptions;

namespace Profiles.Application.Extensions;

public static class Pagination
{
	public static PagedList<T> Paginate<T>(this IEnumerable<T> entities, int page = 1, int pageSize = 10)
	{
		if (page <= 0 || pageSize <= 0)
		{
			throw new BadRequestException("Incorrect paging parameters");
		}

		var maxPage = (int)Math.Ceiling(Convert.ToDecimal(entities.Count()) / pageSize);
		entities = entities.Skip((page - 1) * pageSize).Take(pageSize);

		var hasNext = page < maxPage;
		var hasPrevious = page > 1;

		return new PagedList<T>(entities.ToList(), maxPage, page, hasNext, hasPrevious);
	}
}
