using Profiles.Application.Utility;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Profiles.Infrastructure.Extensions;

public static class OrderByExtension
{
	/// <summary>
	/// This method use Linq.Dynamic.Core to dynamically order some queries. 
	/// OrderByQuery should contain name of ordering propery and asc of desc word to choose ordering direction.
	/// It also can use more that one property, to do that separate them by comma
	/// examples "firstName asc" "birthday desc,firstName asc"
	/// </summary>
	/// <typeparam name="T">Type of object in query</typeparam>
	/// <param name="query">Basic query</param>
	/// <param name="orderByQuery">Order by query string</param>
	/// <param name="defaultOrderPredicate">Default property of generic T which will be used to ordering</param>
	/// <returns></returns>
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
