using Microsoft.Extensions.Caching.Distributed;
using Offices.Core.Cache;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IOfficeRepository> _officeRepository;
	private readonly OfficesContext _context;

	public UnitOfWork(OfficesContext context, ICacheService cacheService)
	{
		_context = context;

		_officeRepository = new Lazy<IOfficeRepository>(() =>
		new OfficeRepository(context, cacheService));
	}

	public IOfficeRepository OfficeRepository => _officeRepository.Value;
}
