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
		return await _context.Offices.ToListAsync();
	}

	public async Task<Office> GetByIdAsync(Guid id, bool trackChanges = false)
	{
		if (trackChanges)
		{
			return await _context
				.Offices
				.FirstOrDefaultAsync(x => x.Id.Equals(id));
		}
		else
		{
			return await _context
				.Offices
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id.Equals(id));
		}
	}
}
