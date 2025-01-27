using Offices.Core.Repositories;
using Offices.Infrastructure.Context;

namespace Offices.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	private readonly Lazy<IRecepcionistRepository> _recepcionistRepository;
	private readonly Lazy<IOfficeRepository> _officeRepository;
	private readonly OfficesContext _context;

    public UnitOfWork(OfficesContext context)
    {
		_context = context;

		_recepcionistRepository = new Lazy<IRecepcionistRepository>(() =>
		new ReceptionistRepository(context));
		
		_officeRepository = new Lazy<IOfficeRepository>(() =>
		new OfficeRepository(context));
    }

    public IRecepcionistRepository RecepcionistRepository => _recepcionistRepository.Value;

	public IOfficeRepository OfficeRepository => _officeRepository.Value;
}
