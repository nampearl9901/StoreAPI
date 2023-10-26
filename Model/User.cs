using MongoDB.Bson.Serialization.Attributes;

namespace StoreAPI.Model
{
    public class User
    {
       

        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ?Email { get; set; }
        public string ?PasswordHash { get; set; }
        public string? LastName { get; set; }
        public string? FristName { get; set; }
        
        public int Age { get; set; }
        public string? Gender { get; set; }  
        public string? Address { get; set; }
        public int Phone { get; set; }
    }
}
