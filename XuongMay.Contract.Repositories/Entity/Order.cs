using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("createdDate")]
        public DateTime CreatedDate { get; set; }

        [BsonElement("finishDate")]
        public DateTime? FinishDate { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("deadline")]
        public DateTime Deadline { get; set; }

        [BsonElement("accountId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId AccountId { get; set; }

        [BsonElement("assignedAccountId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId AssignedAccountId { get; set; }
    }
}
