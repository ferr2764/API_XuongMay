using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Product
    {
        public string ProductId { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        public string ProductSize { get; set; } = null!;

        public string Status { get; set; } = null!;

        public string CategoryId { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}