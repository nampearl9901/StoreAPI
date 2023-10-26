using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Model
{
  
    public class Products
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [MaxLength(100)]
        public string ?ProductName { get; set; }

        [MaxLength(500)]
        public string ?Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }

        public int Quantity { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? BrandId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }

    }
}
