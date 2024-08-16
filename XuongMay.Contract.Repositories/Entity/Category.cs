using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Category
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("categoryName")]
    public string CategoryName { get; set; }

    [BsonElement("categoryDescription")]
    public string CategoryDescription { get; set; }
}
