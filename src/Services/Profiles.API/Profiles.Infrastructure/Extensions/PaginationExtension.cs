using Microsoft.EntityFrameworkCore;
using Profiles.Core.Utility;
using Shared.Exceptions;

namespace Profiles.Infrastructure.Extensions;

public static class PaginationExtension
{
	public static async Task<PagedList<T>> PaginateAsync<T>(this IQueryable<T> query, int page = 1, int pageSize = 10)
	{
		if (page <= 0 || pageSize <= 0)
		{
			throw new BadRequestException("Incorrect paging parameters");
		}

		query = query.Skip((page - 1) * pageSize).Take(pageSize);

		var entities = await query.ToListAsync();
		var maxPage = (int)Math.Ceiling(Convert.ToDecimal(entities.Count()) / pageSize);
		var hasNext = page < maxPage;
		var hasPrevious = page > 1;

		return new PagedList<T>(entities, maxPage, page, hasNext, hasPrevious);
	}
}
