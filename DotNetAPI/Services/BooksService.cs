using Books.Models;
using DotNetAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DotNetAPI.Services;

public class BooksService
{
    private readonly IMongoCollection<Book> _booksCollection;

    // MongoDB Database Settings.
    public BooksService(IOptions<LLMDatabaseSettings> lLMDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            lLMDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            lLMDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<Book>(
            lLMDatabaseSettings.Value.BooksCollectionName);
    }

    // Get List of Books with pagination.
    public async Task<List<Book>> GetAsync(int skip, int limit)
    {
        // return await _booksCollection.Find(_ => true).ToListAsync();
        return await _booksCollection.Find(_ => true)
            .Skip(skip)
            .Limit(limit)
            .ToListAsync();
    }

    // Get total count of Books.
    public async Task<int> GetCountAsync()
    {
        return (int)await _booksCollection.CountDocumentsAsync(_ => true);
    }

    // Get Book by ID.
    public async Task<Book?> GetAsync(string id) =>
        await _booksCollection.Find(x => x.ID == id).FirstOrDefaultAsync();

    // Create a new Book.
    public async Task CreateAsync(Book newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    // Update Book.
    public async Task UpdateAsync(string id, Book updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.ID == id, updatedBook);

    // Delete Book.
    public async Task RemoveAsync(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.ID == id);
}