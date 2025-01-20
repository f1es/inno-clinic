using Microsoft.EntityFrameworkCore;
using Offices.Core.Models;
using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class OfficeRepository : BaseRepository<Office>, IOfficeRepository
{
    public OfficeRepository(OfficesDbContext context)
		: base(context)
    {
        
    }

    public async Task<IEnumerable<Office>> GetAllAsync()
	{
		return await _context.Offices.AsNoTracking().ToListAsync();
	}

	public async Task<Office> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		var table = _context.Offices;
		var office = trackChanges ? table.FirstOrDefaultAsync(x => x.Id == id) : table.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
		return await office;
	}
}
