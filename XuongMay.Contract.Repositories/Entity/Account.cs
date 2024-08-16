using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public class Account
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("role")]
        public string? Role { get; set; }

        [BsonElement("salary")]
        public int? Salary { get; set; }
    }
}
