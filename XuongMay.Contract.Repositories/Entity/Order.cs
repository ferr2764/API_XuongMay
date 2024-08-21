using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace XuongMay.Contract.Repositories.Entity
{
    public partial class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public DateTime CreatedDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string Status { get; set; } = null!;
        public DateTime Deadline { get; set; }
        public Guid AccountId { get; set; }
        public Guid AssignedAccountId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Account AssignedAccount { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
