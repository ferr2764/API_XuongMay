using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("productSize")]
        public string ProductSize { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId CategoryId { get; set; }
    }
}