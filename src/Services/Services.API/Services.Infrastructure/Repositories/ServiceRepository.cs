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

    public async Task<IEnumerable<Service>> GetAllAsync() => await _context.Services.ToListAsync();

	public async Task<Service> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var query = trackChanges ? _context.Services : _context.Services.AsNoTracking();
		return await query.FirstOrDefaultAsync(x => x.Id == id);
	}
}
