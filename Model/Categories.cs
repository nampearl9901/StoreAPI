using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace StoreAPI.Model
{
    public class Categories
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        [MaxLength(100)]
        public string? CategoryName { get; set; }

    
    }
}
