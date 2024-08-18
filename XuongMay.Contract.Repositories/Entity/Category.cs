using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace XuongMay.Contract.Repositories.Entity
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("categoryName")]
        public string CategoryName { get; set; }

        [BsonElement("categoryDescription")]
        public string CategoryDescription { get; set; }

        [BsonElement("categoryStatus")]
        public string CategoryStatus { get; set; }
    }
}
