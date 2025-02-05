using Microsoft.EntityFrameworkCore;
using Profiles.Core.Utility;
using Shared.Exceptions;

namespace Profiles.Infrastructure.Extensions;

public static class PaginationExtension
{
	public static async Task<PagedList<T>> PaginateAsync<T>(this IQueryable<T> entities, int page = 1, int pageSize = 10)
	{
		if (page <= 0 || pageSize <= 0)
		{
			throw new BadRequestException("Incorrect paging parameters");
		}

		var maxPage = (int)Math.Ceiling(Convert.ToDecimal(entities.Count()) / pageSize);
		entities = entities.Skip((page - 1) * pageSize).Take(pageSize);

		var hasNext = page < maxPage;
		var hasPrevious = page > 1;

		return new PagedList<T>(await entities.ToListAsync(), maxPage, page, hasNext, hasPrevious);
	}
}
