using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class OrderDetail
    {
        public Guid DetailId { get; set; } = Guid.NewGuid();
        public int NumberOfProds { get; set; }
        public string Status { get; set; } = null!;
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}