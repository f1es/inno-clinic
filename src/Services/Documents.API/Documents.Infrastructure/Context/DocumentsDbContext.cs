using Documents.Core.Models;
using Documents.Infrastructure.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Documents.Infrastructure.Context;

public class DocumentsDbContext
{
	private readonly IMongoDatabase _database;

    public DocumentsDbContext(IOptions<MongoDbOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        _database = client.GetDatabase(options.Value.DatabaseName);
    }

    public IMongoCollection<Document> Documents => _database.GetCollection<Document>("Documents");

    public IMongoCollection<Photo> Photos => _database.GetCollection<Photo>("Photos");
}
