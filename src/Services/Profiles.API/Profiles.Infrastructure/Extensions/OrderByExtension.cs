using Profiles.Application.Utility;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Profiles.Infrastructure.Extensions;

public static class OrderByExtension
{
	public static IQueryable<T> Order<T>(this IQueryable<T> query, string orderByQuery, Expression<Func<T, object>> defaultOrderPredicate)
	{
		if (string.IsNullOrEmpty(orderByQuery))
		{
			return query.OrderBy(defaultOrderPredicate);
		}

		var orderString = OrderQueryBuilder.BuildSortQuery<T>(orderByQuery);

		if (string.IsNullOrWhiteSpace(orderString))
		{
			return query.OrderBy(defaultOrderPredicate);
		}

		return query.OrderBy(orderString);
	}
}
