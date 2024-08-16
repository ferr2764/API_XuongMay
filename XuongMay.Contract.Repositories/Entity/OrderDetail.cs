using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public class OrderDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("numberOfProds")]
        public int NumberOfProds { get; set; }

        [BsonElement("status")]
        public string OrderDetailStatus { get; set; }

        [BsonElement("orderId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId OrderId { get; set; }
    }
}