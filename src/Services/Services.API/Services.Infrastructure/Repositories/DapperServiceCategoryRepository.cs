using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Services.Core.Models;
using Services.Core.Repositories;
using Services.Infrastructure.Context;
using System.Data;
using static Dapper.SqlMapper;

namespace Services.Infrastructure.Repositories
{
	public class DapperServiceCategoryRepository : IBaseRepository<ServiceCategory>, IServiceCategoryRepository
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;

        public DapperServiceCategoryRepository(ServicesDbContext servicesDbContext)
        {
			_connection = servicesDbContext.Database.GetDbConnection();
			servicesDbContext.Database.BeginTransaction();
			_transaction = servicesDbContext.Database.CurrentTransaction.GetDbTransaction();
		}

		public void Create(ServiceCategory entity)
		{
			var sql = @"INSERT INTO dbo.ServiceCategories (Id, CategoryName, TimeSlotSize)
						VALUES (@Id, @CategoryName, @TimeSlotSize);";

			var sqlParams = new { Id = entity.Id, CategoryName = entity.CategoryName, TimeSlotSize = entity.TimeSlotSize };

			_connection.Execute(sql, sqlParams, _transaction);
		}

		public void Delete(ServiceCategory entity)
		{
			var sql = @"DELETE FROM dbo.ServiceCategories
						WHERE Id = @Id;";

			var sqlParams = new { Id = entity.Id };

			_connection.Execute(sql, sqlParams, _transaction);
		}

		public async Task<IEnumerable<ServiceCategory>> GetAllAsync(CancellationToken cancellationToken)
		{
			var sql = @"SELECT * FROM dbo.ServiceCategories;";

			var command = new CommandDefinition(
				commandText: sql,
				cancellationToken: cancellationToken,
				transaction: _transaction
			);

			return await _connection.QueryAsync<ServiceCategory>(command);
		}

		public async Task<ServiceCategory> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool trackChanges = false)
		{
			var sql = @"SELECT * FROM dbo.ServiceCategories
						WHERE Id = @Id;";

			var sqlParams = new { Id = id };

			var command = new CommandDefinition(
				commandText: sql,
				parameters: sqlParams,
				transaction: _transaction,
				cancellationToken: cancellationToken
			);

			return await _connection.QueryFirstOrDefaultAsync<ServiceCategory>(command);
		}

		public void Update(ServiceCategory entity)
		{
			var sql = @"UPDATE dbo.ServiceCategories
						SET categoryName = @categoryName, timeSlotSize = @timeSlotSize
						WHERE Id = @Id;";

			var sqlParams = new { categoryName = entity.CategoryName, timeSlotSize = entity.TimeSlotSize, Id = entity.Id };

			_connection.Execute(sql, sqlParams, _transaction);
		}
	}
}
