using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class OrderDetail
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("numberOfProds")]
    public int NumberOfProds { get; set; }

    [BsonElement("status")]
    public string Status { get; set; }

    [BsonElement("orderId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId OrderId { get; set; }
}
