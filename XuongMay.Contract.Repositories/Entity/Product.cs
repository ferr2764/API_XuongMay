using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductName { get; set; } = null!;
        public string ProductSize { get; set; } = null!;
        public string Status { get; set; } = null!;
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}