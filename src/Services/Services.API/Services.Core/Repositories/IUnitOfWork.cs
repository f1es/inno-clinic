namespace Services.Core.Repositories;

public interface IUnitOfWork
{
	public IServiceRepository ServiceRepository { get; }
	public IServiceCategoryRepository ServiceCategoryRepository { get; }
	public Task SaveAsync();
}
