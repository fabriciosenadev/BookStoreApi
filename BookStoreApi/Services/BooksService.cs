using BookStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BookStoreApi.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _bookCollection;

        public BooksService(IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            /**
             * fields of appsettings json to connect mongoDb
             * "BookStoreDatabase": {
             *      "ConnectionString": "mongodb://localhost:27017",
             *      "DatabaseName": "BookStore",
             *      "BooksCollectionName": "Books"
             *  }
             * 
             * BookStoreDatabaseSettings translation class from json settings file
             */

            // define connection string 
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            // define database
            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            // define collection will be used
            _bookCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);
        }

        public async Task<List<Book>> GetAsync()
        {
            var result = await _bookCollection.Find(_ => true).ToListAsync();
            return result;
        }

        public async Task<Book?> GetAsync(string id)
        {
            var result = await _bookCollection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return result;
        }

        public async Task CreateAsync(Book newBook)
        {
            await _bookCollection.InsertOneAsync(newBook);
        }

        public async Task UpdateAsync(string id, Book updatedBook)
        {
            /**
             * method UpdateOneAsync is used to update field by field
             * when is necessary to update entire document 
             * the most approppriated to be used is ReplaceOneAsync
             * 
             * how to update a field
             * https://stackoverflow.com/questions/45496613/mongodb-updateone
             * 
             * how to update document
             * https://stackoverflow.com/questions/30257013/mongodb-c-sharp-driver-2-0-update-document
             */
            await _bookCollection.ReplaceOneAsync(x => x.Id.Equals(id), updatedBook);
        }

        public async Task RemoveAsync(string id)
        {
            await _bookCollection.DeleteOneAsync(x => x.Id.Equals(id));
        }
    }
}
