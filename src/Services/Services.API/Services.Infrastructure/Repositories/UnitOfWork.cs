using Services.Core.Repositories;
using Services.Infrastructure.Context;

namespace Services.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IServiceRepository> _serviceRepository;
	private readonly Lazy<IServiceCategoryRepository> _serviceCategoryRepository;
	private readonly ServicesDbContext _context;
    public UnitOfWork(ServicesDbContext context)
    {
        _context = context;

		_serviceRepository = new Lazy<IServiceRepository>(() =>
		new ServiceRepository(context));

		_serviceCategoryRepository = new Lazy<IServiceCategoryRepository>(() =>
		new DapperServiceCategoryRepository(context));
    }

    public IServiceRepository ServiceRepository =>_serviceRepository.Value;

	public IServiceCategoryRepository ServiceCategoryRepository => _serviceCategoryRepository.Value;

	public async Task SaveAsync(CancellationToken cancellationToken) 
	{
		await _context.SaveChangesAsync(cancellationToken);

		if (_context.Database.CurrentTransaction is not null)
		{
			await _context.Database.CurrentTransaction.CommitAsync(cancellationToken);
		}
	}
}
