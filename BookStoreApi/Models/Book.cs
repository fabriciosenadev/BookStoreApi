using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BookStoreApi.Models
{
    public class Book
    {
        [BsonId] // determina que será a chave primária na collection 
        [BsonRepresentation(BsonType.ObjectId)] // tradução para o ObjectId do mongoDb
        public string?  Id { get; set; }

        [BsonElement("Name")] // nome da propriedade dentro da collection do mongoDb
        public string BookName { get; set; } = null;
        public decimal Price { get; set; }
        public string Category { get; set; } = null;
        public string Author { get; set; } = null;
    }
}
