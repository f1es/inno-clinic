using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Offices.Core.Models;
using Offices.Infrastructure.Options;

namespace Offices.Infrastructure.Context;

public class OfficesContext
{
	private readonly IMongoDatabase _database;

    public OfficesContext(IOptions<MongoDbSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Office> Offices => _database.GetCollection<Office>("Offices");
    public IMongoCollection<Receptionist> Receptionists => _database.GetCollection<Receptionist>("Receptionists");
}
