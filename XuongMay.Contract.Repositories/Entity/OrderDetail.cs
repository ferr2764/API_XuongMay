using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class OrderDetail
    {
        public string DetailId { get; set; } = null!;

        public int NumberOfProds { get; set; }

        public string Status { get; set; } = null!;

        public string OrderId { get; set; } = null!;

        public string ProductId { get; set; } = null!;

        public virtual Order Order { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}