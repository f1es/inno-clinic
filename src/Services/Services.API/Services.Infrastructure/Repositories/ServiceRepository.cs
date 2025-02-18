using Microsoft.EntityFrameworkCore;
using Services.Core.Models;
using Services.Core.Repositories;
using Services.Infrastructure.Context;

namespace Services.Infrastructure.Repositories;

public class ServiceRepository : BaseRepository<Service>, IServiceRepository
{
    public ServiceRepository(ServicesDbContext context)
		: base(context)
    { }

    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken) => 
		await _context.Services.AsNoTracking().ToListAsync(cancellationToken);

	public async Task<IEnumerable<Service>> GetAllWithCategoryAsync(CancellationToken cancellationToken) =>
		await _context.Services.AsNoTracking().Include(x => x.ServiceCategory).ToListAsync(cancellationToken);

	public async Task<Service> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Services : _context.Services.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}

	public async Task<Service> GetByIdWithCategoryAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Services : _context.Services.AsNoTracking();
		return await query.Include(x => x.ServiceCategory).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}
}
