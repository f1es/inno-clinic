using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Core.Models;
using Offices.Infrastructure.Options;

namespace Offices.Infrastructure.Context;

public class OfficesContext
{
	private readonly IMongoDatabase _database;
    private readonly IMongoClient _mongoClient;

    public OfficesContext(IMongoClient mongoClient, IOptions<MongoDbSettings> options)
    {
		_mongoClient = mongoClient;
        _database = mongoClient.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Office> Offices => _database.GetCollection<Office>("Offices");
}
