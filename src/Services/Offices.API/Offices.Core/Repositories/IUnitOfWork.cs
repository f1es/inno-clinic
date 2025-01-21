namespace Offices.Core.Repositories;

public interface IUnitOfWork
{
	public IRecepcionistRepository RecepcionistRepository { get; }
	public IOfficeRepository OfficeRepository { get; }
}
