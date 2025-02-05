using Documents.Core.Models;
using Documents.Core.Repositories;
using Documents.Infrastructure.Context;
using MongoDB.Driver;

namespace Documents.Infrastructure.Repositories;

public class PhotoRepository : IPhotoRepository
{
	private readonly DocumentsDbContext _context;

    public PhotoRepository(DocumentsDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Photo photo) => await _context.Photos.InsertOneAsync(photo);

	public async Task DeleteAsync(Guid id) => await _context.Photos.DeleteOneAsync(Builders<Photo>.Filter.Eq(x => x.Id, id));

	public async Task<IEnumerable<Photo>> GetAllAsync() => (await _context.Photos.FindAsync(Builders<Photo>.Filter.Empty)).ToList();

	public async Task<Photo> GetByIdAsync(Guid id) => (await _context.Photos.FindAsync(Builders<Photo>.Filter.Eq(x => x.Id, id))).FirstOrDefault();

	public async Task UpdateAsync(Photo photo) => await _context.Photos.ReplaceOneAsync(Builders<Photo>.Filter.Eq(x => x.Id, photo.Id), photo);
}
