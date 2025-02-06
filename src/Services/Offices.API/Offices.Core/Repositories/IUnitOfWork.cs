namespace Offices.Core.Repositories;

public interface IUnitOfWork
{
	public IOfficeRepository OfficeRepository { get; }
}
