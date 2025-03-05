using Microsoft.EntityFrameworkCore;
using Services.Core.Models;
using Services.Core.Repositories;
using Services.Infrastructure.Context;

namespace Services.Infrastructure.Repositories;

public class ServiceCategoryRepository : BaseRepository<ServiceCategory>, IServiceCategoryRepository
{
    public ServiceCategoryRepository(ServicesDbContext context)
		: base(context)
	{ }

	public async Task<IEnumerable<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken) => await _context.ServiceCategories.ToListAsync();

	public async Task<ServiceCategory> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var query = trackChanges ? _context.ServiceCategories : _context.ServiceCategories.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
